using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IReportContext
    {
        DataTable GetAllReportCharts();
        DataRow GetReportByID(int _reportID);
        bool SaveNewReport(Report _report);
        bool DeletReport(int _reportID);
        DataTable GetReportData(string _query);
        DataTable GetOrderedReport();
    }
}
