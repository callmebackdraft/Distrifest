﻿using System;
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

        public void EditProduct(Product _product)
        {
            Prodctx.EditProduct(_product);
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

        public int SaveNewProduct(Product _product)
        {
            return Prodctx.SaveNewProduct(_product);
        }

        public int UpdateAmountInStock(Product _product, int _amount)
        {
            return Prodctx.UpdateAmountInStock(_product, _amount);
        }

        private Product DataRowToProduct(DataRow _dataRow)
        {
            return new Product(Convert.ToInt32(_dataRow.Field<decimal>("ID")), _dataRow.Field<string>("Type"), _dataRow.Field<string>("Name"), Convert.ToInt32(_dataRow.Field<decimal>("Volume")), _dataRow.Field<string>("VolumeType"), _dataRow.Field<bool>("Active"), Convert.ToInt16(_dataRow.Field<decimal>("AmountInStock")));
        }
    }
}
