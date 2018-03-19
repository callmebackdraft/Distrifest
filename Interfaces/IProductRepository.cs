using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        List<Product> GetAllProducts(int _orderID);
        Product GetProductByID(int _productID);
        int UpdateAmountInStock(Product _product, int _amount);
        void EditProduct(Product product);
    }
}
