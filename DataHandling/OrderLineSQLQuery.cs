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
    public class OrderLineSQLQuery : IOrderLineContext
    {
        public DataTable GetAllOrderLinesForOrder(int _orderID)
        {
            string query = "SELECT * FROM Order_Product WHERE OrderID = @OrderID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query,parameterlist);
        }

        public bool AddOrderLineToOrder(OrderLine _orderLine, int _orderID)
        {
            string query = "INSERT INTO Order_Product(OrderID, ProductID, Amount) VALUES (@OrderID, @ProductID, @Amount)";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID),
                new KeyValuePair<string, object>("@OrderID", _orderLine.Product.ID),
                new KeyValuePair<string, object>("@OrderID", _orderLine.Amount)
            };
            return SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameterlist);
        }
    }
}
