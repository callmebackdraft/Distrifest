using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Report
    {
        public string ChartTitle { get; private set; }
        public string ChartType { get; private set; }
        public string ChartID { get; private set; }
        public string ChartVar { get; private set; }
        public Dictionary<string, int> ChartData { get; private set; }
        public string Query { get; private set; }
    }
}
