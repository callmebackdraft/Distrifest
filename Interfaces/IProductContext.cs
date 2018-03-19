using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IProductContext
    {
        DataTable GetAllProducts();
        DataTable GetAllProducts(int _orderID);
        DataRow GetProductByID(int _productID);
        int UpdateAmountInStock(Product _product,int Amount);
        void EditProduct(Product _product);
        int SaveNewProduct(Product _product);
    }
}
