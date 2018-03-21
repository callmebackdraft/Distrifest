using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public DataTable GetOrderedReport()
        {
            string query =
                @"DECLARE @ColumnNames NVARCHAR(MAX) = ''
                DECLARE @SQL NVARCHAR(MAX) = ''
                
                SELECT @ColumnNames += QUOTENAME(ID) + ','
                FROM Product

                SET @ColumnNames = LEFT(@ColumnNames,LEN(@ColumnNames) -1)
                SET @SQL = 
                'SELECT * 
				FROM (
					SELECT p.ID AS ProductNaam, sq1.Name AS [Gebruiker], op.Amount
						FROM
							Product AS p INNER JOIN
							Order_Product AS op ON p.ID = op.ProductID INNER JOIN
							(SELECT O.ID, U.Name, MAX(OS.Status) AS Status
								FROM 
									Orders AS O INNER JOIN
									Users AS U ON o.CustomerID = u.ID INNER JOIN
									OrderStatus AS OS ON o.ID = os.OrderID
								GROUP BY O.ID, U.Name ) AS SQ1 ON op.OrderID = SQ1.ID
								WHERE SQ1.Status != 4 AND SQ1.Status != 0
					) AS BaseData 
                PIVOT (
	                SUM(Amount)
	                FOR ProductNaam IN (' + @ColumnNames + ')
                ) AS PivotTable'
                EXECUTE(@SQL)";
            DataTable result = SQL_CRUD_Methods.SQLRead(query);
            DataRow _dr;
            foreach (DataColumn _dc in result.Columns)
            {
                try
                {
                    _dr = new ProductSQLQuery().GetProductByID(Convert.ToInt16(_dc.ColumnName));
                    _dc.ColumnName = Regex.Replace(_dr.Field<string>("Name"), "([a-z?])[_ ]?([A-Z])", "$1 $2") + " " + _dr.Field<decimal>("Volume") + " " + _dr.Field<string>("VolumeType");
                }
                catch (Exception exc)
                {

                }
            }
            return result;
        }
    }
}
