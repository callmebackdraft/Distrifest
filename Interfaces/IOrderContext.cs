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
        DataTable GetAllOrders();
        DataTable GetOrderByID(int _orderID);
        int RegisterNewOrder(int _customerID);
        bool ProcessOrder(Order _order);
        bool AddProductToOrder(int _orderID, int _productID, int _productAmount);
    }
}
