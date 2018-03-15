using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using DataHandling;
using Models;
using System.Data;

namespace Repositories
{
    public class DeliveryLineRepository : IDeliveryLineRepository
    {
        public List<DeliveryLine> GetAllDeliveryLines()
        {
            throw new NotImplementedException();
        }

        public List<DeliveryLine> GetAllDeliveryLinesForDelivery(int _deliveryID)
        {
            throw new NotImplementedException();
        }

        public bool SaveDeliveryLine(int _orderId, DeliveryLine _deliveryLine)
        {
            throw new NotImplementedException();
        }

        private DeliveryLine DataRowToDeliveryLine(DataRow _dataRow)
        {
            throw new NotImplementedException();
        }
    }
}
