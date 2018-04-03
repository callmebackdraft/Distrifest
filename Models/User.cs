using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : IEquatable<User>
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

        public bool Equals(User other)
        {
            if (other == null)
                return false;

            return ID == other.ID &&
                string.Equals(Name,other.Name) &&
                string.Equals(Role, other.Role) &&
                bool.Equals(Active, other.Active);
                
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ ID;
                var myStrHashCode =
                    !string.IsNullOrEmpty(Name) ?
                        Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ ID;
                hashCode =
                    (hashCode * 397) ^ Active.GetHashCode();
                return hashCode;
            }
        }
    }
}
