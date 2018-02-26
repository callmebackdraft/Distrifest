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
        bool SaveOrderStatusForOrder(int _orderID, OrderStatus _orderStatus);

    }
}
