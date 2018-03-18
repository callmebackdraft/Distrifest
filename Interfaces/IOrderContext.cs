using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IOrderContext
    {
        DataTable GetAllRelevantOrders();
        DataTable GetAllOrders();
        DataRow GetOrderByID(int _orderID);
        int RegisterNewOrder(int _customerID);
        
        bool AddProductToOrder(int _orderID, int _productID, int _productAmount);
        DataTable CheckForOpenOrder(int _userID);
        void FurtherOrderStatus(int _id, OrderStatus.OrderStatusesEnum _orderStatus);
        DataTable GetAllOrders(int _customerID);
        void UpdateOrder(Order _order);
    }
}
