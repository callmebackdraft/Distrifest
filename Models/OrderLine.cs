using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderLine
    {
        public Product Product { get; private set; }
        public int Amount { get; private set; }
    }
}
