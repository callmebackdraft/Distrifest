using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistriFest.DataHandling;

namespace DistriFest.Models
{
    public class ProductOrderViewModel
    {
        public List<OrderViewModel> OrderDetails { get; private set; }

        public ProductOrderViewModel(int UserID)
        {
            SQLConnect sql = new SQLConnect();
            OrderDetails = sql.GetProductOrder(UserID);
        }

    }
}