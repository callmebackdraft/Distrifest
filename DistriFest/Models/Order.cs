using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistriFest.DataHandling;

namespace DistriFest.Models
{
    public class Order
    {
        public Product Product { get; private set; }
        public int OrderID { get; private set; }
        public int AmountOrdered { get; private set; }
        public string Status { get; private set; }
        public DateTime DateTime { get; private set; }


        public static void RegisterOrder(int OrderID, int ProductID, int ProductAmount)
        {
            SQLConnect.RegisterOrder(OrderID, ProductID, ProductAmount);
        }

        public static List<Order> GetActiveOrderList(int orderID)
        {
            List<Order> orderlist = new List<Order>();
            List<Tuple<Product, int, int, string, DateTime>> DataList = SQLConnect.GetOrderDetails(orderID);

            foreach (Tuple<Product, int, int, string, DateTime> tuple in DataList)
            {
                orderlist.Add(new Order
                {
                    Product = tuple.Item1,
                    OrderID = tuple.Item2,
                    AmountOrdered = tuple.Item3,
                    Status = tuple.Item4,
                    DateTime = tuple.Item5,
                });

            }
            

            return orderlist;
        }

        public static void ProcessOrder(int userID)
        {
            SQLConnect.ProcessOrder(userID);
        }

        internal static void RemoveProduct(int userID, int prodiD)
        {
            SQLConnect.RemoveProductFromOrder(userID, prodiD);
        }

        internal static void EditOrderedAmount(int userID, int prodID, int amountOrdered)
        {
            SQLConnect.ChangeProductAmountInOrder(userID, prodID, amountOrdered);
        }
    }
}