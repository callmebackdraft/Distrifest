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
    }
}
