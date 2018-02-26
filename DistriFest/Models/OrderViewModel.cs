using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public int AmountOrdered { get; set; }
        public Product Product { get; set; }
        public int ProdID { get; set; }

    }
}