using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace DataHandling
{
    public class OrderLineSQLQuery : IOrderLineContext
    {
        public DataTable GetAllOrderLinesForOrder(int _orderID)
        {
            throw new NotImplementedException();
        }
    }
}
