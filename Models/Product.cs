using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; private set; }
        public int Volume { get; private set; }
        public string VolumeType { get; private set; }
        public bool Active { get; private set; }
        public int AmountInStock { get; private set; }

        public Product(int _id, string _name, int _volume, string _volumeType, bool _active, int _amountInStock)
        {
            ID = _id;
            Name = _name;
            Volume = _volume;
            VolumeType = _volumeType;
            Active = _active;
            AmountInStock = _amountInStock;
        }

        public Product(int _id, string _type, string _name, int _volume, string _volumeType, bool _active, int _amountInStock)
        {
            ID = _id;
            Type = _type;
            Name = _name;
            Volume = _volume;
            VolumeType = _volumeType;
            Active = _active;
            AmountInStock = _amountInStock;
        }

        public Product()
        {

        }
    }
}
