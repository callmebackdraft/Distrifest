using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IOrderLineContext
    {
        DataTable GetAllOrderLinesForOrder(int _orderID);
        bool AddOrderLineToOrder(OrderLine _orderLine, int _orderID);
    }
}
