using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace DataHandling
{
    public class OrderSQLQuery : IOrderContext
    {
        public DataTable GetAllOrders()
        {
            string query = "SELECT * FROM Orders";
            return SQL_CRUD_Methods.SQLRead(query);
        }

        public DataRow GetOrderByID(int _orderID)
        {
            string query = "SELECT * FROM Orders WHERE ID = @OrderID";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameters).Rows[0];
        }

        public void FurtherOrderStatus(int _orderID, OrderStatus.OrderStatusesEnum _orderStatus)
        {
            string query = "INSERT INTO [OrderStatus](Status, DateTime, OrderID) VALUES (@Status, @DateTime, @OrderID)";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@Status", _orderStatus),
                new KeyValuePair<string, object>("@DateTime", DateTime.Now),
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameters);
        }

        public int RegisterNewOrder(int _customerID)
        {
            string query = "INSERT INTO [Orders](CustomerID) VALUES (@CustomerID); SELECT SCOPE_IDENTITY();";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@CustomerID", _customerID)
            };
            return SQL_CRUD_Methods.SQLInsert(query, parameterlist);
        }

        public bool AddProductToOrder(int _orderID, int _productID, int _productAmount)
        {
            string query = "INSERT INTO [Order_Product](OrderID, ProductID, Amount) VALUES (@OrderID, @ProductID, @Amount)";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID),
                new KeyValuePair<string, object>("@ProductID", _productID),
                new KeyValuePair<string, object>("@Amount", _productAmount)
            };
            return SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameters);
        }

        public DataTable CheckForOpenOrder(int _userID)
        {
            string query =
                @"SELECT Q.Max_Status AS [Status] ,MIN(Q.OrderID) AS OrderID, Q.CustomerID FROM (
                SELECT MAX (Status) AS [Max_Status], OrderID, O.CustomerID
                FROM OrderStatus AS OS INNER JOIN 
                (SELECT *
                FROM Orders) AS O ON OS.OrderID = O.ID
                WHERE CustomerID = @UserID 
                Group BY OrderID, O.CustomerID
                ) AS Q
                WHERE q.Max_Status = 0
                GROUP BY Q.Max_Status, Q.CustomerID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@UserID", _userID),
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist);
        }

        public DataTable GetAllOrders(int _customerID)
        {
            string query = "SELECT * FROM Orders WHERE CustomerID = @CustomerID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@CustomerID", _customerID),
            };
            return SQL_CRUD_Methods.SQLRead(query,parameterlist);
        }

        public void UpdateOrder(Order _order)
        {
            string query = "UPDATE Orders SET CustomerID = @CustomerID WHERE ID = @OrderID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@CustomerID", _order.CustomerID),
                new KeyValuePair<string, object>("@OrderID", _order.ID)
            };
            SQL_CRUD_Methods.SQLUpdate(query,parameterlist);
        }

        public DataTable GetAllRelevantOrders()
        {
            string query = "SELECT t1.* FROM Orders t1 INNER JOIN (SELECT   MAX( Status ) AS max_total, OrderID FROM OrderStatus GROUP BY OrderID) t2 ON t1.ID = t2.OrderID WHERE t2.max_total > 0 AND t2.max_total < 3";
            return SQL_CRUD_Methods.SQLRead(query);
        }
    }
}
