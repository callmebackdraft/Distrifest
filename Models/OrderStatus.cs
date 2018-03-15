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
        public OrderStatusesEnum RegisteredStatus { get; private set; }
        public DateTime RegisteredDate { get; private set; }

        public OrderStatus(OrderStatusesEnum _registeredStatus, DateTime _registeredDate, int _orderID)
        {
            RegisteredStatus = _registeredStatus;
            RegisteredDate = _registeredDate;
            OrderID = _orderID;
        }
        public enum OrderStatusesEnum
        {
            Ordering,
            WaitingForDC,
            Processing,
            Delivered,
            Rejected
        }
    }
}
