using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories;
using Interfaces;

namespace DistriFest.Models.ViewModels
{
    public class OrderViewModel
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public List<OrderLineViewModel> Products { get; set; }
        public List<OrderStatusViewModel> Statuses { get; set; }
        public OrderStatus.OrderStatusesEnum SelectedStatus { get; set; }

        public OrderViewModel()
        {

        }

        public OrderViewModel(Order _order)
        {
            ID = _order.ID;
            CustomerID = _order.CustomerID;
            Products = new List<OrderLineViewModel>();
            IProductRepository ProdRepo = new ProductRepository();
            foreach(Product _prod in ProdRepo.GetAllProducts())
            {
                Products.Add(new OrderLineViewModel(new OrderLine(_prod,0)));
            }
            Statuses = new List<OrderStatusViewModel>();
            for (int i = 0; i < 3; i++)
            {
                Statuses.Add(new OrderStatusViewModel(new OrderStatus((OrderStatus.OrderStatusesEnum)i,DateTime.Now,ID)));
            }
        }

        public Order ConvertToOrder()
        {
            List<OrderLine> OrderLines = new List<OrderLine>();
            foreach(OrderLineViewModel _olvm in Products)
            {
                OrderLines.Add(_olvm.ConvertToOrderLine());
            }
            return new Order(ID, CustomerID, OrderLines);
        }
    }
}