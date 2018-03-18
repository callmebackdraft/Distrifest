using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandling
{
    class SQL_CRUD_Methods
    {
        private static SqlConnection EstablishConnection()
        {
            SqlConnection DBConnection = new SqlConnection(Properties.Settings.Default.MSSQLConnString);
            try
            {
                DBConnection.Open();
                return DBConnection;
            }
            catch (SqlException exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private static void CloseConnection(SqlConnection _sqlConn)
        {
            _sqlConn.Close();
        }


        private static SqlCommand BuildSQLCommand(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlConnection conn = EstablishConnection();
            SqlCommand sqlcmd = new SqlCommand(query, conn);
            foreach (KeyValuePair<string, object> kvp in parameterlist)
            {
                sqlcmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
            }
            if (!CheckQueryValidity(sqlcmd, conn))
            {
                //throw new SqlQueryException("Invalid Query, check the query for typo's");
            }
            return sqlcmd;
        }

        private static SqlCommand BuildSQLCommand(string query)
        {
            SqlConnection conn = EstablishConnection();
            SqlCommand sqlcmd = new SqlCommand(query, conn);
            if (!CheckQueryValidity(sqlcmd, conn))
            {
                //throw new SqlQueryException("Invalid Query, check the query for typo's");
            }
            return sqlcmd;
        }

        private static bool CheckQueryValidity(SqlCommand commandToCheck, SqlConnection conn)
        {
            bool result;
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("SET NOEXEC ON", conn);
                cmd.ExecuteNonQuery();
                commandToCheck.ExecuteNonQuery();
                result = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                result = false;
            }
            finally
            {
                cmd = new SqlCommand("SET NOEXEC OFF", conn);
                cmd.ExecuteNonQuery();
            }
            return result;
        }

        public static int SQLInsert(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            int result = Convert.ToInt16(sqlComm.ExecuteScalar());
            CloseConnection(sqlComm.Connection);
            return result;
        }

        public static bool SQLInsertBoolReturn(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            int result = sqlComm.ExecuteNonQuery();
            CloseConnection(sqlComm.Connection);
            return (result > 0);
        }

        public static bool SQLCreate(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            throw new NotImplementedException();
        }

        public static bool SQLUpdate(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            int result = sqlComm.ExecuteNonQuery();
            CloseConnection(sqlComm.Connection);
            return (result > 0);
        }

        public static int SQLUpdateReturnInt(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            int result = Convert.ToInt16(sqlComm.ExecuteScalar());
            CloseConnection(sqlComm.Connection);
            return result;
        }

        public static bool SQLDelete(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            int result = sqlComm.ExecuteNonQuery();
            CloseConnection(sqlComm.Connection);
            return (result > 0); ;
        }

        public static DataTable SQLRead(string query)
        {
            var result = new DataTable();
            SqlCommand sqlComm = BuildSQLCommand(query);
            using (var da = new SqlDataAdapter(sqlComm))
            {
                da.Fill(result);
            }
            CloseConnection(sqlComm.Connection);
            return result;
        }

        public static DataTable SQLRead(string query, List<KeyValuePair<string, object>> parameterlist)
        {
            var result = new DataTable();
            SqlCommand sqlComm = BuildSQLCommand(query, parameterlist);
            using (var da = new SqlDataAdapter(sqlComm))
            {
                da.Fill(result);
            }
            CloseConnection(sqlComm.Connection);
            return result;
        }
    }
}
