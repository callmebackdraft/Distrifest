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
    public class DeliverySQLQuery : IDeliveryContext
    {
        public DataTable GetAllDeliverys()
        {
            string query = "SELECT * FROM Delivery";
            return SQL_CRUD_Methods.SQLRead(query);
        }

        public DataRow GetDeliveryByID(int _deliveryID)
        {
            string query = "SELECT * FROM Delivery WHERE ID = @DeliveryID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@DeliveryID",_deliveryID)
            };
            return SQL_CRUD_Methods.SQLRead(query,parameterlist).Rows[0];
        }

        public int GetNewDelivery()
        {
            string query = "INSERT INTO Delivery(DateTime,ExternalID) VALUES (@DateTime,@ExternalID); SELECT SCOPE_IDENTITY()";
            List<KeyValuePair<string, Object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@DateTime", DateTime.Now),
                new KeyValuePair<string, object>("@ExternalID","X")
            };
            return SQL_CRUD_Methods.SQLInsert(query,parameterlist);
        }

        public void UpdateDelivery(Delivery _delivery)
        {
            string query = "UPDATE Delivery SET DateTime = @DateTime, ExternalID = @ExternalID WHERE ID = @DeliveryID";
            List<KeyValuePair<string, Object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@DateTime",DateTime.Now),
                new KeyValuePair<string, object>("@ExternalID",_delivery.ExternalID),
                new KeyValuePair<string, object>("@DeliveryID",_delivery.ID)
            };
            SQL_CRUD_Methods.SQLUpdate(query, parameterlist);
        }
    }
}
