using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Interfaces;
using DataHandling;
using System.Data;

namespace Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        IDeliveryContext Delctx;
        public DeliveryRepository()
        {
            Delctx = new DeliverySQLQuery();
        }
        public List<Delivery> GetAllDeliverys()
        {
            List<Delivery> result = new List<Delivery>();
            foreach(DataRow _dr in Delctx.GetAllDeliverys().Rows)
            {
                result.Add(DataRowToDelivery(_dr));
            }
            return result;
        }

        public Delivery GetDeliveryByID(int _deliveryID)
        {
            return DataRowToDelivery(Delctx.GetDeliveryByID(_deliveryID));
        }

        public bool SaveNewDelivery()
        {
            return Delctx.SaveNewDelivery();
        }

        private Delivery DataRowToDelivery(DataRow _dataRow)
        {
            IDeliveryLineRepository DelLineRepo = new DeliveryLineRepository();
            return new Delivery(Convert.ToInt16(_dataRow.Field<decimal>("ID")),DelLineRepo.GetAllDeliveryLinesForDelivery(Convert.ToInt16(_dataRow.Field<decimal>("ID"))),_dataRow.Field<DateTime>("DateTime"));
        }
    }
}
