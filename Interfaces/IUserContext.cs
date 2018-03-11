using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserContext
    {
        DataRow GetUserByID(int _userID);
        DataTable GetAllUsers();
    }
}
