using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IOrderLineRepository
    {
        List<OrderLine> GetAllOrderLinesForOrder(int _orderID);
        bool AddOrderLineToOrder(OrderLine _orderLine,int _orderID);
    }
}
