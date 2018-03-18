using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDeliveryLineContext
    {
        DataTable GetAllDeliveryLines();
        DataTable GetAllDeliveryLinesForDelivery(int _deliveryID);
        bool SaveDeliveryLine(int _deliveryID, DeliveryLine _deliveryLine);
        void SaveAllDeliveryLines(Delivery _delivery);
    }
}
