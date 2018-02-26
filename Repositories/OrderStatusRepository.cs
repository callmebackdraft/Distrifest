using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;
using System.Data;
using DataHandling;

namespace Repositories
{
    class OrderStatusRepository : IOrderStatusRepository
    {
        IOrderStatusContext OrderStatusCtx;

        public OrderStatusRepository()
        {
            OrderStatusCtx = new OrderStatusSQLQuery();
        }
        public List<OrderStatus> GetOrderStatusesForOrder(int _orderID)
        {
            List<OrderStatus> result = new List<OrderStatus>();
            DataTable rawData = OrderStatusCtx.GetOrderStatusesForOrder(_orderID);
            foreach (DataRow dr in rawData.Rows)
            {
                result.Add(DataRowToOrderStatus(dr));
            }
            return result;
        }

        public bool SaveOrderStatusForOrder(int _orderID, OrderStatus _orderStatus)
        {
            return OrderStatusCtx.SaveOrderStatusForOrder(_orderID,_orderStatus);
        }

        private OrderStatus DataRowToOrderStatus(DataRow _dataRow)
        {
            throw new NotImplementedException();
        }
    }
}
