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
        void RemoveOrderLineFromOrder(int iD1, int iD2);
        void EditOrderedAmount(int prodID, int orderID, int amount);
    }
}
