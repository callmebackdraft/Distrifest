using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.DataHandling
{
    public class ReportChart
    {
        public string ChartTitle { get; private set; }
        public string ChartType { get; private set; }
        public string ChartID { get; private set; }
        public string ChartVar { get; private set; }
        public Dictionary<string,int> ChartData { get; private set; }
        public string Query { get; private set; }

        public ReportChart(string title, string type, string id, string var)
        {
            ChartTitle = title;
            ChartType = type;
            ChartID = id;
            ChartVar = var;
        }

        public void AddChartData( Dictionary<string, int> data)
        {
            ChartData = data;
        }

        public void AddQuery(string query)
        {
            Query = query;
        }
    }
}