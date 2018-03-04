using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Repositories;
using Interfaces;

namespace DistriFest.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Order Order { get; private set; }
        public int UserID { get; private set; }

        public ShoppingCartViewModel(int _userID)
        {
            IOrderRepository OrderRepo = new OrderRepository();
            UserID = _userID;
            Order = OrderRepo.CheckForOpenOrder(_userID); ;
        }
    }
}