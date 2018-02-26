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

        public DataTable GetOrderByID(int _orderID)
        {
            string query = "SELECT * FROM Orders WHERE ID = @OrderID";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameters);
        }

        public bool ProcessOrder(Order _order)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewOrder(int _customerID)
        {
            throw new NotImplementedException();
        }

        public bool AddProductToOrder(int _orderID, int _productID, int _productAmount)
        {
            throw new NotImplementedException();
        }
    }
}
