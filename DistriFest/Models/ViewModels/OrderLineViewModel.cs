using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DistriFest.Models.ViewModels
{
    public class OrderLineViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Amount { get; set; }

        public OrderLineViewModel()
        {

        }

        public OrderLineViewModel(OrderLine _orderLine)
        {
            Product = new ProductViewModel(_orderLine.Product);
            Amount = _orderLine.Amount;
        }

        public OrderLine ConvertToOrderLine()
        {
            return new OrderLine(Product.ConvertToProduct(), Amount);
        }
    }
}