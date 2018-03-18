using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DistriFest.Models.ViewModels
{
    public class DeliveryLineViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Amount { get; set; }

        public DeliveryLineViewModel()
        {
                
        }

        public DeliveryLineViewModel(DeliveryLine _deliveryLine)
        {
            Product = new ProductViewModel(_deliveryLine.Product);
            Amount = _deliveryLine.Amount;
        }

        public DeliveryLineViewModel(ProductViewModel _product, int _amount)
        {
            Product = _product;
            Amount = _amount;
        }

        internal DeliveryLine ConvertToDeliveryLine()
        {
            return new DeliveryLine(Product.ConvertToProduct(), Amount);
        }
    }
}
