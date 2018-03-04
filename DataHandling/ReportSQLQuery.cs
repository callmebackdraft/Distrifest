using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Models;

namespace DataHandling
{
    public class ReportSQLQuery : IReportContext
    {
        public bool DeletReport(int _reportID)
        {
            throw new NotImplementedException();
        }
        public DataTable GetAllReportCharts()
        {
            string query = "SELECT * FROM Chart";
            return SQL_CRUD_Methods.SQLRead(query);
        }
        public DataRow GetReportByID(int _reportID)
        {
            string query = "SELECT * FROM Chart WHERE ID = @ChartID";
            List<KeyValuePair<string, object>> parameterlist = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("@ChartID", _reportID)
            };
            return SQL_CRUD_Methods.SQLRead(query, parameterlist).Rows[0];
        }
        public DataTable GetReportData(string _query)
        {
            return SQL_CRUD_Methods.SQLRead(_query);
        }
        public bool SaveNewReport(Report _report)
        {
            throw new NotImplementedException();
        }
    }
}
