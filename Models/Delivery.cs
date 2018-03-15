using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Delivery
    {
        public int ID { get; private set; }
        public List<DeliveryLine> Products { get; private set; }
        public DateTime DeliveryTime { get; private set; }

        public Delivery(int _id, List<DeliveryLine> _products, DateTime _deliveryTime)
        {
            ID = _id;
            Products = _products;
            DeliveryTime = _deliveryTime;
        }
    }
}
