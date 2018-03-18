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
        public string ExternalID { get; private set; }

        public Delivery()
        {

        }
        public Delivery(int _id, List<DeliveryLine> _products, DateTime _deliveryTime)
        {
            ID = _id;
            Products = _products;
            DeliveryTime = _deliveryTime;
        }

        public Delivery(int _id, List<DeliveryLine> _products, DateTime _deliveryTime,string _externalID)
        {
            ID = _id;
            Products = _products;
            DeliveryTime = _deliveryTime;
            ExternalID = _externalID;
        }

        public void SetProductsList(List<DeliveryLine> _products)
        {
            Products = _products;
        }
    }
}
