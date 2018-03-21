using m = Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories;
using Interfaces;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DistriFest.Models.ViewModels
{
    public class OrderViewModel
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public List<OrderLineViewModel> Products { get; set; }
        public List<OrderStatusViewModel> Statuses { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public m.OrderStatus.OrderStatusesEnum SelectedStatus { get; set; }
        [Required(ErrorMessage = "Er dient een bar geselecteerd te worden")]
        [Range(1,int.MaxValue,ErrorMessage = "Er dient een bar geselecteerd te worden")]
        public int SelectedUserID { get; set; }
        

        public OrderViewModel()
        {

        }

        public OrderViewModel(m.Order _order)
        {
            ID = _order.ID;
            CustomerID = _order.CustomerID;
            Products = new List<OrderLineViewModel>();
            IProductRepository ProdRepo = new ProductRepository();
            IUserRepository UserRepo = new UserRepository();
            UserList = new List<SelectListItem>();
            UserList.Add(new SelectListItem { Value = "", Text = "" });
            foreach(m.User _u in UserRepo.GetAllUsers())
            {
                if(_u.Role == "Bar")
                {
                    UserList.Add(new SelectListItem { Value = _u.ID.ToString(), Text = _u.Name });
                }
            }
            foreach(m.Product _prod in ProdRepo.GetAllProducts())
            {
                Products.Add(new OrderLineViewModel(new m.OrderLine(_prod,0)));
            }
            Statuses = new List<OrderStatusViewModel>();
            for (int i = 0; i < 3; i++)
            {
                Statuses.Add(new OrderStatusViewModel(new m.OrderStatus((m.OrderStatus.OrderStatusesEnum)i,DateTime.Now,ID)));
            }
        }

        public m.Order ConvertToOrder()
        {
            List<m.OrderLine> OrderLines = new List<m.OrderLine>();
            foreach(OrderLineViewModel _olvm in Products)
            {
                OrderLines.Add(_olvm.ConvertToOrderLine());
            }
            return new m.Order(ID, SelectedUserID, OrderLines);
        }
    }
}