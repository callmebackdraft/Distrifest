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
    public class OrderRepository : IOrderRepository
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

        public Order CheckForOpenOrder(int _userID)
        {
            DataTable dbResult = Orderctx.CheckForOpenOrder(_userID);
            if(dbResult.Rows.Count > 0 && dbResult.Rows.Count < 2)
            {
                return DataRowToOrder(Orderctx.GetOrderByID(Convert.ToInt16(dbResult.Rows[0].Field<decimal>("ID"))));
            }
            else
            {
                return RegisterNewOrder(_userID);
            }
        }

        public void FurtherOrderStatus(Order _order, OrderStatus.OrderStatusesEnum _orderStatus)
        {
            if (_orderStatus == OrderStatus.OrderStatusesEnum.WaitingForDC)
            {
                foreach (OrderLine _ol in _order.Products)
                {
                    int ActualAmount = new ProductRepository().UpdateAmountInStock(_ol.Product, _ol.Amount * -1);
                    if (ActualAmount != (_ol.Amount * -1))
                    {
                        new OrderLineRepository().EditOrderedAmount(_order.ID,_ol.Product.ID, Math.Abs(ActualAmount));
                    }
                }
            }
            else if(_orderStatus == OrderStatus.OrderStatusesEnum.Rejected)
            {
                foreach(OrderLine _ol in _order.Products)
                {
                    new ProductRepository().UpdateAmountInStock(_ol.Product,_ol.Amount);
                }
            }

            Orderctx.FurtherOrderStatus(_order.ID, _orderStatus);
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

        public List<Order> GetAllOrders(int _customerID)
        {
            List<Order> result = new List<Order>();
            foreach (DataRow _dr in Orderctx.GetAllOrders(_customerID).Rows)
            {
                result.Add(DataRowToOrder(_dr));
            }
            return result;

        }

        public Order GetOrderByID(int _orderID)
        {
            return DataRowToOrder(Orderctx.GetOrderByID(_orderID));
        }

        public Order RegisterNewOrder(int _customerID)
        {
            int newOrderID = Orderctx.RegisterNewOrder(_customerID);
            IOrderStatusRepository OrderStatusRepo = new OrderStatusRepository();
            Order result = DataRowToOrder(Orderctx.GetOrderByID(newOrderID));
            result.AddOrderStatus(OrderStatusRepo.GenerateOrderStatus(newOrderID, OrderStatus.OrderStatusesEnum.Ordering));
            return result;
        }

        public void UpdateOrder(Order _order)
        {
            Orderctx.UpdateOrder(_order);
        }

        private Order DataRowToOrder(DataRow _dataRow)
        {
            int OrderID = Convert.ToInt16(_dataRow.Field<decimal>("ID"));
            IOrderLineRepository OrderLineRepo = new OrderLineRepository();
            IOrderStatusRepository OrderStatusRepo = new OrderStatusRepository();
            Order result = new Order(OrderID, Convert.ToInt16(_dataRow.Field<Decimal>("CustomerID")));
            foreach (OrderLine OL in OrderLineRepo.GetAllOrderLinesForOrder(OrderID))
            {
                result.AddOrderLine(OL);
            }
            foreach (OrderStatus OS in OrderStatusRepo.GetOrderStatusesForOrder(OrderID))
            {
                result.AddOrderStatus(OS);
            }
            return result;
        }

    }
}
