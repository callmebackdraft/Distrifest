using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        public int ID { get; private set; }
        public User Customer { get; private set; }
        //public int CustomerID { get; private set; }
        public List<OrderLine> Products {get; private set;}
        public List<OrderStatus> Statuses { get; private set; }

        public Order(int _id, User _customer, List<OrderLine> _products, List<OrderStatus> _orderStatuses)
        {
            InstantiateLists();

            ID = _id;
            Customer = _customer;
            Products = _products;
            Statuses = _orderStatuses;
        }

        public Order(int _id, User _customer, List<OrderLine> _products)
        {
            InstantiateLists();

            ID = _id;
            Customer = _customer;
            Products = _products;
        }

        public Order(int _id, User _customer)
        {
            InstantiateLists();
            ID = _id;
            Customer = _customer;
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
