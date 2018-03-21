using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace DataHandling
{
    public class UserSQLQuery : IUserContext
    {
        public DataTable GetAllUsers()
        {
            string query = "SELECT ID, Name, Role, Active FROM Users";
            return SQL_CRUD_Methods.SQLRead(query);
        }

        public DataRow GetUserByID(int _userID)
        {
            string query = "SELECT ID, Name, Role, Active FROM Users WHERE ID = @UserID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@UserID",_userID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist).Rows[0];
        }
    }
}
