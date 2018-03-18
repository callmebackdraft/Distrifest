using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IOrderRepository
    {

        List<Order> GetAllOrders();
        List<Order> GetAllOrders(int _customerID);
        Order GetOrderByID(int _orderID);
        Order RegisterNewOrder(int _customerID);
        bool AddProductToOrder(int _orderID, int _productID, int _productAmount);
        Order CheckForOpenOrder(int _userID);
        void FurtherOrderStatus(Order _order, OrderStatus.OrderStatusesEnum _orderStatus);
        void UpdateOrder(Order order);
    }
}
