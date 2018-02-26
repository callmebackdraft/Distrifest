using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace DistriFest.DataHandling
{
    public class PasswordHandling
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static Tuple<string,bool> ValidatePassword(string password, string username)
        {
            string storedHash = SQLConnect.GetStoredPW(username);
            if (storedHash == null) 
            {
                storedHash = GetRandomSalt();
            }
            Tuple<string, bool> result = new Tuple<string, bool>(storedHash, BCrypt.Net.BCrypt.CheckPassword(password, storedHash));

            return result;
        }
    }
}