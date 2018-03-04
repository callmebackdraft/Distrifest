using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace DataHandling
{
    public class BCryptPasswordHandling
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static Tuple<string, bool> ValidatePassword(string password, string _username)
        {

            string query = "SELECT [Password] FROM [Users] WHERE [Mail] = @User";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@User", _username)
            };
            DataRow queryResult = SQL_CRUD_Methods.SQLRead(query, parameterlist).Rows[0];

            string storedHash = queryResult.Field<string>("Password");
            if (storedHash == null)
            {
                storedHash = GetRandomSalt();
            }
            Tuple<string, bool> result = new Tuple<string, bool>(storedHash, BCrypt.Net.BCrypt.CheckPassword(password, storedHash));
            return result;
        }
    }
}
