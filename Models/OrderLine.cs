using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderLine
    {
        public Product Product { get; set; }
        public int Amount { get; set; }

        public OrderLine()
        {

        }
        public OrderLine(Product _product, int _amount)
        {
            Product = _product;
            Amount = _amount;
        }
    }
}
