using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Reporting;
using TelerikServiceReports;

namespace ProcessPdf
{
    /// <summary>
    /// Summary description for ConvertReportToPdf2
    /// </summary>
    public class ConvertReportToPdf2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string request = context.Request["request"];
            string[] parts = request.Split('~');


            string Query = string.Empty;

            Query = "SELECT Appointment.ID as Id, person.FirstName as FirstName, person.LastName as LastName,"
                + " CONVERT(varchar, [ApptDay], 101) as DateOfAppointment,CONVERT(varchar(15), CAST([ApptTime] AS TIME), 100) as TimeOfAppointment,"
                + " CONVERT(varchar, person.DOB, 101) as DateOfBirth, '0' as Account"
                + " FROM Appointment"
                + " inner join webid"
                + " on webid.ID = appointment.UserID"
                + " inner join person"
                + " on person.ID = webid.PersonID"
                + " where[ApptDay] = '" + parts[0] + "'"
                //+ " where[ApptDay] = '06/26/2020'"
                + " and person.locationid = " + parts[1]
                //+ " and person.locationid = 2"
                + " order by Appointment.ApptDay, Appointment.ApptTime";

            var report = new PrintSchedule();

            Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource();
            sqlDataSource.ConnectionString = "Server = 64.41.86.25; Database = Appointment; Uid = Appointment; Pwd = 0griswold;";
            sqlDataSource.SelectCommand = Query;

            report.DataSource = sqlDataSource;
            var reportPackager = new ReportPackager();
            using (var targetStream = System.IO.File.Create(@"C:\temp\PrintReport.trdp"))
            {
                reportPackager.Package(report, targetStream);
            }


            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            var deviceInfo = new System.Collections.Hashtable();

            var reportSource = new Telerik.Reporting.UriReportSource();

            //// reportName is the path to the TRDP/TRDX file
            reportSource.Uri = @"C:\temp\PrintReport.trdp";


            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

            string filePath = System.IO.Path.Combine(@"C:\waldenltd\ionicwithvuedev\fairfield-app-admin\public\", "output.pdf");

            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}