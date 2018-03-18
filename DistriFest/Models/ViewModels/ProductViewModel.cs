using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public string VolumeType { get; set; }
        public bool Active { get; set; }
        public int AmountInStock { get; set; }


        public ProductViewModel(Product _product)
        {
            ID = _product.ID;
            Name = _product.Name;
            Volume = _product.Volume;
            VolumeType = _product.VolumeType;
            Active = _product.Active;
            AmountInStock = _product.AmountInStock;
        }
        public ProductViewModel(int _id, string _name, int _volume, string _volumeType, bool _active, int _amountInStock)
        {
            ID = _id;
            Name = _name;
            Volume = _volume;
            VolumeType = _volumeType;
            Active = _active;
            AmountInStock = _amountInStock;
        }

        internal Product ConvertToProduct()
        {
            return new Product(ID, Name, Volume, VolumeType, Active, AmountInStock);
        }

        public ProductViewModel()
        {

        }
    }
}
