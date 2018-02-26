using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; private set; }
        public int Volume { get; private set; }
        public string VolumeType { get; private set; }
        public bool Active { get; private set; }

        public Product(int id, string name, int volume, string volumetype, bool active)
        {
            ID = id;
            Name = name;
            Volume = volume;
            VolumeType = volumetype;
            Active = active;
        }

        public Product()
        {

        }
    }

    
}