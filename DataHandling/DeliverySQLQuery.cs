using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

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

        public bool SaveNewDelivery()
        {
            throw new NotImplementedException();
        }
    }
}
