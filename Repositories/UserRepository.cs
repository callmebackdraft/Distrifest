using DataHandling;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        IUserContext Userctx;
        public UserRepository()
        {
            Userctx = new UserSQLQuery();
        }
        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();
            foreach(DataRow dr in Userctx.GetAllUsers().Rows)
            {
                result.Add(DataRowToUser(dr));
            }
            return result;
        }

        public User GetUserByID(int _userID)
        {
            return DataRowToUser(Userctx.GetUserByID(_userID));
        }

        private User DataRowToUser(DataRow _dataRow)
        {
            return new User(Convert.ToInt16(_dataRow.Field<decimal>("ID")), _dataRow.Field<string>("Name"), _dataRow.Field<bool>("Active"));
        }
    }
}
