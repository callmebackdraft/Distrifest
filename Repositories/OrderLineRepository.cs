using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Interfaces;
using DataHandling;
using System.Data;

namespace Repositories
{
    public class OrderLineRepository : IOrderLineRepository
    {
        IOrderLineContext OrderLinectx;

        public OrderLineRepository()
        {
            OrderLinectx = new OrderLineSQLQuery();
        }
        public List<Order> GetAllOrderLinesForOrder(int _orderID)
        {
            throw new NotImplementedException();
        }
        private OrderLine DataRowToOrderLine(DataRow _dr)
        {
            throw new NotImplementedException();
        }
    }
}
