using System;
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
        private OrderLine DataRowToOrderLine(DataRow _dr)
        {
            IProductRepository ProdRepo = new ProductRepository();
            return new OrderLine(ProdRepo.GetProductByID(Convert.ToInt16(_dr.Field<decimal>("ProductID"))), Convert.ToInt16(_dr.Field<decimal>("Amount")));
        }
        
    }
}
