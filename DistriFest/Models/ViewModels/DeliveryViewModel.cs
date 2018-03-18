using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DistriFest.Models.ViewModels
{
    public class DeliveryViewModel
    {
        public int ID { get; set; }
        public List<DeliveryLineViewModel> Products { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string ExternalID { get; set; }

        public DeliveryViewModel()
        {

        }

        public DeliveryViewModel(Delivery _delivery)
        {
            ID = _delivery.ID;
            DeliveryTime = _delivery.DeliveryTime;
            ExternalID = _delivery.ExternalID;
            Products = new List<DeliveryLineViewModel>();
            foreach(DeliveryLine _dl in _delivery.Products)
            {
                Products.Add(new DeliveryLineViewModel(_dl));
            }
        }

        public DeliveryViewModel(int _id, List<DeliveryLineViewModel> _products, DateTime _deliveryTime)
        {
            ID = _id;
            Products = _products;
            DeliveryTime = _deliveryTime;
        }

        public void SetProductsList(List<DeliveryLineViewModel> _products)
        {
            Products = _products;
        }

        internal Delivery ConvertToDelivery()
        {
            List<DeliveryLine> DelLines = new List<DeliveryLine>();
            foreach(DeliveryLineViewModel DLVM in Products)
            {
                DelLines.Add(DLVM.ConvertToDeliveryLine());
            }
            return new Delivery(ID,DelLines, DeliveryTime,ExternalID);
        }
    }
}
