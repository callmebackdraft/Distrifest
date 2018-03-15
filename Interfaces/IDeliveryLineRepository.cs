using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Models;

namespace Interfaces
{
    public interface IDeliveryLineRepository
    {
        List<DeliveryLine> GetAllDeliveryLines();
        List<DeliveryLine> GetAllDeliveryLinesForDelivery(int _deliveryID);
        bool SaveDeliveryLine(int _orderId, DeliveryLine _deliveryLine);
    }
}
