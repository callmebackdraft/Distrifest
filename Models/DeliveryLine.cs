using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DeliveryLine
    {
        public Product Product { get; private set; }
        public int Amount { get; private set; }

        public DeliveryLine(Product _product, int _amount)
        {
            Product = _product;
            Amount = _amount;
        }
    }
}
