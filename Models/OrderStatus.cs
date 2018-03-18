using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        public string GetDescription()
        {
            FieldInfo fi = RegisteredStatus.GetType().GetField(RegisteredStatus.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return RegisteredStatus.ToString();
            }

        }
        public enum OrderStatusesEnum
        {
            [Description("Bezig Met Bestellen")]
            Ordering,
            [Description("In Wachtrij bij DC")]
            WaitingForDC,
            [Description("In Behandeling bij DC")]
            Processing,
            [Description("Geleverd")]
            Delivered,
            [Description("Geweigerd")]
            Rejected
        }
    }
}
