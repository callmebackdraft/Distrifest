﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Interfaces;
using DataHandling;
using System.Data;

namespace Repositories
{
    public class OrderLineRepository : IOrderLineRepository
    {
        IOrderLineContext OrderLinectx;

        public OrderLineRepository()
        {
            OrderLinectx = new OrderLineSQLQuery();
        }
        public List<OrderLine> GetAllOrderLinesForOrder(int _orderID)
        {
            List<OrderLine> result = new List<OrderLine>();
            foreach (DataRow dr in OrderLinectx.GetAllOrderLinesForOrder(_orderID).Rows)
            {
                result.Add(DataRowToOrderLine(dr));
            }
            return result;
        }
        public bool AddOrderLineToOrder(OrderLine _orderLine, int _orderID)
        {
            return OrderLinectx.AddOrderLineToOrder(_orderLine, _orderID);
        }
        public void RemoveOrderLineFromOrder(int _prodID, int _orderID)
        {
            OrderLinectx.RemoveOrderLineFromOrder(_prodID, _orderID);
        }
        public void EditOrderedAmount(int _orderID, int _prodID, int _amount)
        {
            OrderLinectx.EditOrderedAmount(_prodID, _orderID, _amount);
        }
        private OrderLine DataRowToOrderLine(DataRow _dr)
        {
            IProductRepository ProdRepo = new ProductRepository();
            return new OrderLine(ProdRepo.GetProductByID(Convert.ToInt16(_dr.Field<decimal>("ProductID"))), Convert.ToInt16(_dr.Field<decimal>("Amount")));
        }

        public void SaveAllOrderLines(Order _order)
        {
            foreach(OrderLine _ol in _order.Products)
            {
                if(_ol.Amount != 0)
                {
                    AddOrderLineToOrder(_ol, _order.ID);
                }
            }
        }
    }
}
