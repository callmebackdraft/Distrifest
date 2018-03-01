using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        public int ID { get; private set; }
        public List<Product> Products {get; private set;}
        public List<OrderStatus> OrderStatuses { get; private set; }
        
    }
}
