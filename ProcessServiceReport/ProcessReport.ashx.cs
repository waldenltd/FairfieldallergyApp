using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Reporting;

namespace ProcessServiceReport
{
    /// <summary>
    /// Summary description for ProcessReport
    /// </summary>
    public class ProcessReport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string values = context.Request.QueryString["values"];

            string[] parts = values.Split('~');

            TelerikServiceReports.ProcessReports processReports = new TelerikServiceReports.ProcessReports();
            processReports.ProcessServiceReports(parts[0], parts[1], parts[2].Replace("ZZZ","&"), parts[3], parts[4], parts[5], parts[6], parts[7]);

            context.Response.ContentType = "text/plain";
            context.Response.Write("Success");
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