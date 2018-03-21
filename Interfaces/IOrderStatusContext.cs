using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IOrderStatusContext
    {
        DataTable GetOrderStatusesForOrder(int _orderID);
        bool SaveOrderStatusForOrder(OrderStatus _orderStatus);
        DataTable GetAllStatuses();
    }
}
