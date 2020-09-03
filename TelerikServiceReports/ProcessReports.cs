using Dapper;
using System;
using System.Data;
using System.Linq;
using Telerik.Reporting;
using System.Data.SqlClient;

namespace TelerikServiceReports
{
    public class ProcessReports
    {
        public void ProcessServiceReports(string fromDate, string toDate, string company,
            string totalBillable, string balanceFromPreviousMonth,
                string hoursPurchasedThisMonth, string balancedRemaining,string month)
        {

            string Query = string.Empty;

            var report = new ServiceReport(totalBillable, balanceFromPreviousMonth, hoursPurchasedThisMonth, balancedRemaining,month);

            Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource();
            sqlDataSource.ConnectionString = "Server=ITD-CW02.ITD.local;User ID=reports;password=C0nn3ct3d;Database=cwwebapp_itd;Persist Security Info=True";
            sqlDataSource.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sqlDataSource.SelectCommand = "ITD_ServiceRpt";
            sqlDataSource.Parameters.Add("@FromDate", (System.Data.DbType)TypeCode.String, fromDate);
            sqlDataSource.Parameters.Add("@ToDate", (System.Data.DbType)TypeCode.String, toDate);
            sqlDataSource.Parameters.Add("@company", (System.Data.DbType)TypeCode.String, company);

            report.DataSource = sqlDataSource;
            var reportPackager = new ReportPackager();
            using (var targetStream = System.IO.File.Create("C:\\temp\\ServiceReport.trdp"))
            {
                reportPackager.Package(report, targetStream);
            }
        }
    }

    public class Results
    {
        public string Agreement { get; set; }
        public string Company_Name { get; set; }
        public DateTime AsOfDate { get; set; }
        public double PrevBalanceHrs { get; set; }
        public double BalanceHours { get; set; }
        public double AllocatedHours { get; set; }
        public double BillableHours { get; set; }
        public double CarryForwardHours { get; set; }
        
    }

//    Agreement Company_Name    AsOfDate PrevBalanceHrs  BalanceHours AllocatedHours  BillableHours CarryForwardHours   TotalBillableHrs BalanceRemaining
//1025.002 MSP Bronze Lite Friendship Tours	2019-03-01	47.22	36.52	6.00	16.70	0.00	16.20	37.02

}
