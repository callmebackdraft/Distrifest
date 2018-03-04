using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IReportRepository
    {
        List<Report> GetAllReportCharts();
        Report GetReportByID(int _reportID);
        bool SaveNewReport(Report _report);
        bool DeletReport(int _reportID);
    }
}
