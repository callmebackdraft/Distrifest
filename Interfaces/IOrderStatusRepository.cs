using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IOrderStatusRepository
    {
        List<OrderStatus> GetOrderStatusesForOrder(int _orderID);
        OrderStatus GenerateOrderStatus(int _orderID, OrderStatus.OrderStatusesEnum _orderStatus);
        bool SaveOrderStatusForOrder(OrderStatus _orderStatus);

    }
}
