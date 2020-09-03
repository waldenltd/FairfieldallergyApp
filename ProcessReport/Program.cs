using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting;
using TelerikServiceReports;

namespace ProcessReport
{
    class Program
    {
        static void Main(string[] args)
        {

            string databaseConnection = System.Configuration.ConfigurationManager.AppSettings["allergyConnection"];
            string reportLocation = System.Configuration.ConfigurationManager.AppSettings["reportLocation"];
            string outputPath = System.Configuration.ConfigurationManager.AppSettings["outputPath"];

            string Query = string.Empty;

            Query = "SELECT Appointment.ID as Id,person.HPhone, person.FirstName as FirstName, person.LastName as LastName,"
                + " CONVERT(varchar, [ApptDay], 101) as DateOfAppointment,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as TimeOfAppointment,"
                + " CONVERT(varchar, person.DOB, 101) as DateOfBirth, '0' as Account"
                + " FROM Appointment"
                + " inner join webid"
                + " on webid.ID = appointment.UserID"
                + " inner join person"
                + " on person.ID = webid.PersonID"
                + " where[ApptDay] = '" + args[0] + "'"
                + " and person.locationid = " + args[1]
                + " order by Appointment.ApptDay, Appointment.ApptTime";
            Console.WriteLine(Query);
            var report = new PrintSchedule();


            Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource();
            sqlDataSource.ConnectionString = databaseConnection;
            sqlDataSource.SelectCommand = Query;

            report.DataSource = sqlDataSource;
            var reportPackager = new ReportPackager();
            using (var targetStream = System.IO.File.Create(reportLocation))
            {
                reportPackager.Package(report, targetStream);
            }


            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            var deviceInfo = new System.Collections.Hashtable();

            var reportSource = new Telerik.Reporting.UriReportSource();

            //// reportName is the path to the TRDP/TRDX file
            reportSource.Uri = reportLocation;


            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

            string filePath = System.IO.Path.Combine(outputPath, "output.pdf");

            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }
        }
    }
}
