using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistriFest.DataHandling;

namespace DistriFest.Models
{
    public class ReportingViewModel
    {
        public List<ReportChart> ReportList { get; private set; }


        public ReportingViewModel()
        {
            SQLConnect sql = new SQLConnect();
            ReportList = sql.GetReportData();
        }

        public void AddChart()
        {
            SQLConnect sql = new SQLConnect();
            ReportChart chart = new ReportChart("A", "A", "A", "A");
            chart.AddQuery("Select [Name] From dbo.Product");
            //sql.AddNewReport(chart);
        }
    }
}