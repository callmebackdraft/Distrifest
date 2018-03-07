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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Order> GetAllOrders();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_orderID"></param>
        /// <returns></returns>
        Order GetOrderByID(int _orderID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customerID"></param>
        /// <returns></returns>
        Order RegisterNewOrder(int _customerID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_orderID"></param>
        /// <param name="_productID"></param>
        /// <param name="_productAmount"></param>
        /// <returns></returns>
        bool AddProductToOrder(int _orderID, int _productID, int _productAmount);
        /// <summary>
        /// Checks for existing open order for specified user, if one is available it returns the open Order else it generates a new Order.
        /// </summary>
        /// <param name="_userID"></param>
        /// <returns>Order</returns>
        Order CheckForOpenOrder(int _userID);
        void FurtherOrderStatus(Order _order, OrderStatus.OrderStatusesEnum _orderStatus);
    }
}
