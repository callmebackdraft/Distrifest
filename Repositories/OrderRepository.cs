using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;
using DataHandling;
using System.Data;

namespace Repositories
{
    class OrderRepository : IOrderRepository
    {
        IOrderContext Orderctx;

        public OrderRepository()
        {
            Orderctx = new OrderSQLQuery();
        }

        public bool AddProductToOrder(int _orderID, int _productID, int _productAmount)
        {
            return Orderctx.AddProductToOrder(_orderID, _productID, _productAmount);
        }

        public List<Order> GetAllOrders()
        {
            List<Order> result = new List<Order>();
            DataTable rawData = Orderctx.GetAllOrders();
            foreach(DataRow dr in rawData.Rows)
            {
                result.Add(DataRowToOrder(dr));
            }
            return result;
        }

        public Order GetOrderByID(int _orderID)
        {
            return DataRowToOrder(Orderctx.GetOrderByID(_orderID).Rows[0]);
        }

        public bool ProcessOrder(Order _order)
        {
            return Orderctx.ProcessOrder(_order.ID);
        }

        public int RegisterNewOrder(int _customerID)
        {
            return Orderctx.RegisterNewOrder(_customerID);
        }

        private Order DataRowToOrder(DataRow _dataRow)
        {
            throw new NotImplementedException();
        }

    }
}
