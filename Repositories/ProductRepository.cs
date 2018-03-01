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
    public class ProductRepository : IProductRepository
    {
        IProductContext Prodctx;
        public ProductRepository()
        {
            Prodctx = new ProductSQLQuery();
        }
        public List<Product> GetAllProducts()
        {
            List<Product> result = new List<Product>();
            foreach (DataRow dr in Prodctx.GetAllProducts().Rows)
            {
                result.Add(DataRowToProduct(dr));
            }
            return result;
        }

        public List<Product> GetAllProducts(int _orderID)
        {
            List<Product> result = new List<Product>();
            foreach (DataRow dr in Prodctx.GetAllProducts(_orderID).Rows)
            {
                result.Add(DataRowToProduct(dr));
            }
            return result;
        }

        public Product GetProductByID(int _productID)
        {
            return DataRowToProduct(Prodctx.GetProductByID(_productID));
        }

        private Product  DataRowToProduct(DataRow _dataRow)
        {

            throw new NotImplementedException();
        }
    }
}
