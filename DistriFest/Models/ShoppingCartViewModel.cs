using DistriFest.DataHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Models
{
    public class ShoppingCartViewModel
    {
        public List<Order> OrderList { get; private set; }
        public int OrderID { get; set; }
        public int UserID { get; private set; }

        public ShoppingCartViewModel(int userID)
        {
            UserID = userID;
            OrderList = Order.GetActiveOrderList(SQLConnect.CheckCreateActiveOrder(userID));
        }
    }
}