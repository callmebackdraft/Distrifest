using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderStatus
    {
        public int OrderID { get; private set; }
        public OrderStatuses RegisteredStatus { get; private set; }
        public DateTime RegisteredDate { get; private set; }

        public OrderStatus(OrderStatuses _registeredStatus, DateTime _registeredDate)
        {
            RegisteredStatus = _registeredStatus;
            RegisteredDate = _registeredDate;
        }
        public enum OrderStatuses
        {
            Ordering,
            Processing,
            EnRoute,
            Delivered
        }
    }
}
