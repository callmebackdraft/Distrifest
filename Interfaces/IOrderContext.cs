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
        DataRow GetOrderByID(int _orderID);
        int RegisterNewOrder(int _customerID);
        bool ProcessOrder(int _orderID);
        bool AddProductToOrder(int _orderID, int _productID, int _productAmount);
    }
}
