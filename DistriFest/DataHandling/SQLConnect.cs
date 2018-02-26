using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistriFest.Models;
using DistriFest.Exceptions;

namespace DistriFest.DataHandling
{
    public class SQLConnect
    {
        internal static void RegisterOrder(int orderID, int productID, int productAmount)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "IF EXISTS (SELECT 1 FROM dbo.Order_Product WHERE OrderID = @OrderID AND ProductID = @ProductID) BEGIN UPDATE dbo.Order_Product SET Amount = Amount + @Amount WHERE OrderID = @OrderID AND ProductID = @ProductID END ELSE BEGIN INSERT INTO Order_Product(OrderID,ProductID,Amount) Values (@OrderID,@ProductID,@Amount) END";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@OrderID", orderID);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@Amount", productAmount);
            cmd.ExecuteNonQuery();
            DBConnection.Close();
        }

        internal static List<Tuple<Product,int,int,string,DateTime>> GetOrderDetails(int orderID)
        {
            SqlConnection DBConnection = EstablishConnection();
            List<Tuple<Product, int, int, string, DateTime>> result = new List<Tuple<Product, int, int, string, DateTime>>();
            string query = "SELECT OP.OrderID, P.ID AS ProductID, P.Type, P.Name, P.Volume, P.VolumeType, P.Active, Amount = SUM(OP.Amount) , O.[DateTime], O.[Status] FROM Order_Product AS OP Left JOIN Product AS P ON OP.ProductID = P.ID Left Join dbo.[Order] AS O ON OP.OrderID = O.ID WHERE OP.OrderID = @OrderID GROUP BY OP.OrderID, P.ID, P.Type, P.Name, P.Volume, P.VolumeType, P.AmountInStock, P.Active, O.[DateTime], O.[Status]";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@OrderID", orderID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Product product = new Product(Convert.ToInt16(dr["ProductID"]),dr["Name"].ToString(), Convert.ToInt16(dr["Volume"]),dr["VolumeType"].ToString(),Convert.ToBoolean(dr["Active"]));
                Tuple<Product, int, int, string, DateTime> tuple = new Tuple<Product, int, int, string, DateTime>(product, orderID, Convert.ToInt16(dr["Amount"]), dr["Status"].ToString(),Convert.ToDateTime(dr["DateTime"]));
                result.Add(tuple);
            }
            dr.Close();
            DBConnection.Close();

            return result;
        }

        internal static void ChangeProductAmountInOrder(int userID, int prodID, int amountOrdered)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "UPDATE [dbo].[Order_Product] SET Amount = @amountOrdered WHERE OrderID = @OrderID AND ProductID = @prodID";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@amountOrdered", amountOrdered);
            cmd.Parameters.AddWithValue("@prodID", prodID);
            cmd.Parameters.AddWithValue("@OrderID", CheckCreateActiveOrder(userID));
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch 
            {
                throw new QueryException("");
            }
            DBConnection.Close();
        }

        internal static void RemoveProductFromOrder(int userID, int prodiD)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "DELETE FROM [Order_Product] WHERE ProductID = @prodID AND OrderID = @OrderID";
            SqlCommand cmd = new SqlCommand(query,DBConnection);
            cmd.Parameters.AddWithValue("@prodID", prodiD);
            cmd.Parameters.AddWithValue("@OrderID", CheckCreateActiveOrder(userID));
            cmd.ExecuteNonQuery();
            DBConnection.Close();
        }

        internal static void ProcessOrder(int userID)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "UPDATE [dbo].[Order] SET Status = 'Ordered' WHERE CustomerID = @UserID AND Status = 'Ordering'";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.ExecuteScalar();
            DBConnection.Close();
        }

        public SQLConnect()
        {
            EstablishConnection();
        }

        private static SqlConnection EstablishConnection()
        {
            SqlConnection DBConnection = new SqlConnection
            {
                ConnectionString = Properties.Settings.Default.SecondaryDB
            };

            try
            {
                DBConnection.Open();
                return DBConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e);
                return DBConnection;
            }
        }

        internal string GetRole(string username)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "SELECT ROLE FROM [UserRoles] JOIN [Users] ON [Users].ID = [UserRoles].UserID WHERE Mail = @user";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@user", username);
            DBConnection.Close();
            return (string)cmd.ExecuteScalar();
        }

        public List<OrderViewModel> GetProductOrder(int UserID)
        {
            SqlConnection DBConnection = EstablishConnection();
            List<OrderViewModel> OVMList = new List<OrderViewModel>();
            int orderID = CheckCreateActiveOrder(UserID);
            string query = "select * from dbo.Product";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                Product product = new Product(Convert.ToInt16(dr["ID"]), dr["Name"].ToString(), Convert.ToInt16(dr["Volume"]),dr["VolumeType"].ToString(),Convert.ToBoolean(dr["Active"]));

                OrderViewModel order = new OrderViewModel
                {
                    OrderID = orderID,
                    Product = product,
                    ProdID = product.ID
                };
                

                OVMList.Add(order);
            }
            dr.Close();
            DBConnection.Close();
            return OVMList;
        }


        public List<ReportChart> GetReportData()
        {
            SqlConnection DBConnection = EstablishConnection();
            List<ReportChart> ReportChartList = new List<ReportChart>();
            string query = "Select * from dbo.Chart";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();
                ReportChart chart = new ReportChart(dr["Title"].ToString(), dr["Type"].ToString(), dr["DivID"].ToString(), dr["JSVar"].ToString());
                SqlCommand subCommand = new SqlCommand(dr["Query"].ToString(), DBConnection);
                SqlDataReader sdr = subCommand.ExecuteReader();
                var sdrcolumns = Enumerable.Range(0, sdr.FieldCount).Select(sdr.GetName).ToList();
                while (sdr.Read())
                {
                    dict.Add(sdr[sdrcolumns[0]].ToString(), Convert.ToInt16(sdr[sdrcolumns[1]]));
                }
                chart.AddChartData(dict);
                ReportChartList.Add(chart);
                sdr.Close();
            }
            dr.Close();
            DBConnection.Close();
            return ReportChartList;
        }

        public void AddNewReport(ReportChart chart)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "INSERT INTO [Chart] (Title,DivID,JSVar,Query,Type) Values (@Title,@DivID,@JSVar,@Query,@Type)";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@Title", chart.ChartTitle);
            cmd.Parameters.AddWithValue("@DivID", chart.ChartID);
            cmd.Parameters.AddWithValue("@JSVar", chart.ChartVar);
            cmd.Parameters.AddWithValue("@Query", chart.Query);
            cmd.Parameters.AddWithValue("@Type", chart.ChartType);
            try
            {
                var executeQuery = cmd.ExecuteNonQuery();
            }
            catch (SqlException exc)
            {
                DBConnection.Close();
                switch (exc.Number)
                {
                    case 50000:
                        throw new QueryException(exc.Message);
                    case 50001:
                        throw new QueryException(exc.Message);
                    case 50002:
                        throw new QueryException(exc.Message);
                }

            }

        }

        public static int CheckCreateActiveOrder(int UserID)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "SELECT ID FROM [Order] WHERE [CustomerID] = @UserID AND [Status] = 'Ordering'";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            int OrderID = Convert.ToInt16(cmd.ExecuteScalar());
            if (OrderID == 0)
            {
                query = "INSERT INTO [Order](CustomerID, Status, DateTime) VALUES (@UserID, @Status, @DateTime); SELECT SCOPE_IDENTITY();";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Status", "Ordering");
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                OrderID = Convert.ToInt16(cmd.ExecuteScalar());
            }
            DBConnection.Close();
            return OrderID;
        }

        public bool CheckAuth(string user, string password)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "SELECT CASE  WHEN EXISTS (SELECT * FROM [Users] WHERE Mail = '@user' AND Password = '@pass') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@user", "dennis.aspers@gmail.com");
            cmd.Parameters.AddWithValue("@pass", "blaat");
            DBConnection.Close();
            return Convert.ToBoolean(cmd.ExecuteScalar());
        }

        public static string GetStoredPW(string username)
        {
            SqlConnection DBConnection = EstablishConnection();
            string query = "SELECT [Password] FROM [Users] WHERE [Mail] = @user";
            SqlCommand cmd = new SqlCommand(query, DBConnection);
            cmd.Parameters.AddWithValue("@user", username);
            return (string)cmd.ExecuteScalar();
        }
    }
}