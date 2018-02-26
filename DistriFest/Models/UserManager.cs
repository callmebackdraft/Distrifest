using DistriFest.DataHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Models
{
    public class UserManager
    {
        public bool IsValid(string username, string password)
        {
            return new SQLConnect().CheckAuth(username, password);
        }

        public string GetRole(string username, string password)
        {
            return new SQLConnect().GetRole(username);
        }
    }
}