using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IDeliveryContext
    {
        DataTable GetAllDeliverys();
        DataRow GetDeliveryByID(int _deliveryID);
        int GetNewDelivery();
        void UpdateDelivery(Delivery _delivery);
    }
}
