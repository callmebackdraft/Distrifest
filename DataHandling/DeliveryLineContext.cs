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
    public class DeliveryLineContext : IDeliveryLineContext
    {
        public DataTable GetAllDeliveryLines()
        {
            string query = "SELECT * FROM Delivery_Product";
            return SQL_CRUD_Methods.SQLRead(query);
        }

        public DataTable GetAllDeliveryLinesForDelivery(int _deliveryID)
        {
            string query = "SELECT * FROM Delivery_Product WHERE DeliveryID = @DeliveryID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@DeliveryID",_deliveryID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist);
        }

        public void SaveAllDeliveryLines(Delivery _delivery)
        {
            foreach(DeliveryLine DelLine in _delivery.Products)
            {
                if (DelLine.Amount > 0)
                {
                    SaveDeliveryLine(_delivery.ID, DelLine);
                }
            }
        }

        public bool SaveDeliveryLine(int _deliveryID, DeliveryLine _deliveryLine)
        {
            string query = "INSERT INTO Delivery_Product(DeliveryID,ProductID,Amount) VALUES (@DeliveryID,@ProductID,@Amount)";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@DeliveryID",_deliveryID),
                new KeyValuePair<string, object>("@ProductID",_deliveryLine.Product.ID),
                new KeyValuePair<string, object>("@Amount",_deliveryLine.Amount)
            };
            return SQL_CRUD_Methods.SQLInsertBoolReturn(query, parameterlist);
        }
    }
}
