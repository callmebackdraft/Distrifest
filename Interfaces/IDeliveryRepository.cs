using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IDeliveryRepository
    {
        List<Delivery> GetAllDeliverys();
        Delivery GetDeliveryByID(int _deliveryID);
        bool SaveNewDelivery();
    }
}
