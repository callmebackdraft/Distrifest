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

        public OrderStatus GenerateOrderStatus(int _orderID,OrderStatus.OrderStatusesEnum _orderStatus)
        {
            OrderStatus result = new OrderStatus(_orderStatus, DateTime.Now, _orderID);
            SaveOrderStatusForOrder(result);
            return result;
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

        public bool SaveOrderStatusForOrder(OrderStatus _orderStatus)
        {
            return OrderStatusCtx.SaveOrderStatusForOrder(_orderStatus);
        }

        private OrderStatus DataRowToOrderStatus(DataRow _dataRow)
        {
            return new OrderStatus((OrderStatus.OrderStatusesEnum)Enum.Parse(typeof(OrderStatus.OrderStatusesEnum), _dataRow.Field<string>("Status")), _dataRow.Field<DateTime>("DateTime"), Convert.ToInt16(_dataRow.Field<decimal>("OrderID")));
        }
    }
}
