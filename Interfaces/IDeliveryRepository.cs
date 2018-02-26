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
        Delivery GetAllDeliverys();
        List<Delivery> GetDeliveryByID();
        bool SaveNewDelivery();
    }
}
