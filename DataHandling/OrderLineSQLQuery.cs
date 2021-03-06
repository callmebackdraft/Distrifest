﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace DataHandling
{
    public class OrderLineSQLQuery : IOrderLineContext
    {
        public DataTable GetAllOrderLinesForOrder(int _orderID)
        {
            string query = "SELECT * FROM Order_Product WHERE OrderID = @OrderID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query,parameterlist);
        }

        public bool AddOrderLineToOrder(OrderLine _orderLine, int _orderID)
        {
            string query = "INSERT INTO Order_Product(OrderID, ProductID, Amount) VALUES (@OrderID, @ProductID, @Amount)";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID),
                new KeyValuePair<string, object>("@ProductID", _orderLine.Product.ID),
                new KeyValuePair<string, object>("@Amount", _orderLine.Amount)
            };
            return SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameterlist);
        }
        public void RemoveOrderLineFromOrder(int _prodID, int _orderID)
        {
            string query = "DELETE FROM[Order_Product] WHERE ProductID = @ProductID AND OrderID = @OrderID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID),
                new KeyValuePair<string, object>("@ProductID", _prodID),
            };
            SQL_CRUD_Methods.SQLDelete(query, parameterlist);
        }
        public void EditOrderedAmount(int _prodID, int _orderID, int _amount)
        {
            string query = "UPDATE [dbo].[Order_Product] SET Amount = @Amount WHERE OrderID = @OrderID AND ProductID = @ProductID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID", _orderID),
                new KeyValuePair<string, object>("@ProductID", _prodID),
                new KeyValuePair<string, object>("@Amount", _amount)
            };
            SQL_CRUD_Methods.SQLUpdate(query, parameterlist);
        }

        public DataTable GetAllOrderLines()
        {
            string query = "SELECT * FROM Order_Product";
            return SQL_CRUD_Methods.SQLRead(query);
        }
    }
}
