using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using m = Models;

namespace DistriFest.Models.ViewModels
{
    public class UserViewModel  
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public UserViewModel()
        {

        }
        public UserViewModel(m.User _user)
        {
            ID = _user.ID;
            Name = _user.Name;
            Active = _user.Active;
        }
    }
}