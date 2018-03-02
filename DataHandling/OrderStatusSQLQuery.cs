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
    public class OrderStatusSQLQuery : IOrderStatusContext
    {
        public DataTable GetOrderStatusesForOrder(int _orderID)
        {
            string query = "SELECT * FROM OrderStatus WHERE ID = @OrderID";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameters);
        }

        public bool SaveOrderStatusForOrder(OrderStatus _orderStatus)
        {
            string query = "INSERT INTO [OrderStatus](Status, DateTime, OrderID) VALUES (@Status, @DateTime, @OrderID)";
            List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@Status", _orderStatus.RegisteredStatus),
                new KeyValuePair<string, object>("@DateTime", _orderStatus.RegisteredDate),
                new KeyValuePair<string, object>("@OrderID", _orderStatus.OrderID)
            };
            return SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameters);
        }
    }
}
