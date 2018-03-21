using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }
        public bool Active { get; private set; }

        public User(int _iD, string _name, string _role ,bool _active)
        {
            ID = _iD;
            Name = _name;
            Role = _role;
            Active = _active;
        }
    }
}
