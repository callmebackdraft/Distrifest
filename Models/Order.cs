using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        public int ID { get; private set; }
        public int CustomerID { get; private set; }
        public List<OrderLine> Products {get; private set;}
        public List<OrderStatus> Statuses { get; private set; }

        public Order(int _id, int _customerID, List<OrderLine> _products, List<OrderStatus> _orderStatuses)
        {
            InstantiateLists();

            ID = _id;
            CustomerID = _customerID;
            Products = _products;
            Statuses = _orderStatuses;
        }

        public Order(int _id)
        {
            InstantiateLists();
            ID = _id;
        }

        public Order()
        {
            InstantiateLists();
        }

        public void AddOrderLine(OrderLine _orderLine)
        {
            Products.Add(_orderLine);
        }

        public void AddOrderStatus(OrderStatus _orderStatus)
        {
            Statuses.Add(_orderStatus);
        }

        private void InstantiateLists()
        {
            Products = new List<OrderLine>();
            Statuses = new List<OrderStatus>();
        }
    }
}
