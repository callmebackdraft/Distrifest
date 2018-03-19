using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace DataHandling
{
    public class ProductSQLQuery : IProductContext
    {
        public void EditProduct(Product _product)
        {
            string query = "UPDATE Product SET Volume = @Volume, VolumeType = @VolumeType, Active = @Active WHERE ID = @ProductID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@ProductID",_product.ID),
                new KeyValuePair<string, object>("@Volume",_product.Volume),
                new KeyValuePair<string, object>("@VolumeType",_product.VolumeType),
                new KeyValuePair<string, object>("@Active",_product.Active)
            };
            SQL_CRUD_Methods.SQLUpdate(query,parameterlist);
        }

        public DataTable GetAllProducts()
        {
            string query = "SELECT * FROM Product";
            return SQL_CRUD_Methods.SQLRead(query);
        }

        public DataTable GetAllProducts(int _orderID)
        {
            string query = "";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@OrderID",_orderID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist);
        }

        public DataRow GetProductByID(int _productID)
        {
            string query = "SELECT * FROM Product WHERE ID = @ProductID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@ProductID",_productID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist).Rows[0];
        }

        public int UpdateAmountInStock(Product _product, int _amount)
        {
            string query = "DECLARE @Difference int SET @Difference = (SELECT AmountInStock FROM Product WHERE ID = @ProductID) + @AmountDelivered IF @Difference < 0 SET @AmountDelivered = @AmountDelivered - @Difference; UPDATE Product SET AmountInStock = AmountInStock + @AmountDelivered WHERE ID = @ProductID SELECT @AmountDelivered AS 'Result'";
            List<KeyValuePair<string,object>> parameterlist = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("@AmountDelivered", _amount),
                    new KeyValuePair<string, object>("@ProductID",_product.ID)
                };
            return SQL_CRUD_Methods.SQLUpdateReturnInt(query, parameterlist);
        }
    }
}
