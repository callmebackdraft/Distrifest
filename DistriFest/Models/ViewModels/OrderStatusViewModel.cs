using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DistriFest.Models.ViewModels
{
    public class OrderStatusViewModel
    {
        public int OrderID { get; set; }
        public OrderStatus.OrderStatusesEnum RegisteredStatus { get; set; }
        public DateTime RegisteredDate { get; set; }

        public OrderStatusViewModel()
        {

        }

        public OrderStatusViewModel(OrderStatus _orderStatus)
        {
            OrderID = _orderStatus.OrderID;
            RegisteredStatus = _orderStatus.RegisteredStatus;
            RegisteredDate = _orderStatus.RegisteredDate;
        }
    }
}