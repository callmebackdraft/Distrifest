using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace DataHandling
{
    public class ProductSQLQuery : IProductContext
    {
        public DataTable GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public DataTable GetAllProducts(int _orderID)
        {
            throw new NotImplementedException();
        }

        public DataRow GetProductByID(int _productID)
        {
            throw new NotImplementedException();
        }
    }
}
