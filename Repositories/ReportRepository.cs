using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandling;
using Interfaces;
using Models;

namespace Repositories
{
    public class ReportRepository : IReportRepository
    {
        IReportContext Reportctx;
        public ReportRepository()
        {
            Reportctx = new ReportSQLQuery();
        }
        public bool DeletReport(int _reportID)
        {
            throw new NotImplementedException();
        }

        public List<Report> GetAllReportCharts()
        {
            List<Report> result = new List<Report>();
            foreach(DataRow dr in Reportctx.GetAllReportCharts().Rows)
            {
                result.Add(DataRowToReport(dr));
            }
            return result;
        }

        public Report GetReportByID(int _reportID)
        {
            return DataRowToReport(Reportctx.GetReportByID(_reportID));
        }

        public bool SaveNewReport(Report _report)
        {
            return Reportctx.SaveNewReport(_report);
        }

        private Report DataRowToReport(DataRow _dataRow)
        {
            Dictionary<string, int> chartData = new Dictionary<string, int>();
            string dataName = "";
            int dataValue = 0;
            foreach(DataRow dr in Reportctx.GetReportData(_dataRow.Field<string>("Query")).Rows)
            {
                foreach(object cell in dr.ItemArray)
                {
                    if(cell is string)
                    {
                        dataName = (string)cell;
                    }
                    else if (cell is int || cell is decimal)
                    {
                        dataValue = Convert.ToInt16(cell);
                    }
                }
                chartData.Add(dataName, dataValue);
            }
            return new Report(_dataRow.Field<string>("Title"), _dataRow.Field<string>("Type"), _dataRow.Field<string>("DivID"), _dataRow.Field<string>("JSVar"),chartData, _dataRow.Field<string>("Query"));
        }
    }
}
