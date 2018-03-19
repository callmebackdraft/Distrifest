using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DistriFest.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Invullen van Type is verplicht")]
        [DisplayName("Product Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Invullen van Naam is verplicht")]
        [DisplayName("Product Naam")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Hoeveelheid in verpakking")]
        [Range(1,int.MaxValue,ErrorMessage = "Volume moet meer dan 0 zijn")]
        public int Volume { get; set; }
        [Required(ErrorMessage = "Invullen van VolumeType is verplicht")]
        [DisplayName("Inhoud type")]
        public string VolumeType { get; set; }
        [Required]
        [DisplayName("Actief")]
        public bool Active { get; set; }
        [Required]
        [DisplayName("Hoeveelheid op Voorraad")]
        public int AmountInStock { get; set; }


        public ProductViewModel(Product _product)
        {
            ID = _product.ID;
            Type = _product.Type;
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
            return new Product(ID, Type, Name, Volume, VolumeType, Active, AmountInStock);
        }

        public ProductViewModel()
        {

        }
    }
}
