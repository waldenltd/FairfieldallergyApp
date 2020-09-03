using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FairfieldAllergy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Email : ControllerBase
    {
        // GET api/<Email>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {

            string [] parts = id.Split('~');

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            Patient patient = fairfieldAllergeryRepository.GetPatientNameForEmail(parts[0]);
            fairfieldAllergeryRepository.UpdateEmail(parts[0], parts[1]);
            TextWriter htmlDocument = new StreamWriter(@"C:\temp\mail.html");


            htmlDocument.WriteLine("<!DOCTYPE HTML PUBLIC " + Chr(34) + "-//W3C//DTD HTML 4.0 Transitional//EN" + Chr(34) + " > ");
            htmlDocument.WriteLine("<html>");
            htmlDocument.WriteLine("<head>");
            htmlDocument.WriteLine("<meta http-equiv=" + Chr(34) + "Content-Type" + Chr(34) + " content=" + Chr(34) + "text/html; charset=utf-8" + Chr(34) + " />");
            htmlDocument.WriteLine("<title>Single-Column Responsive Email Template</title>");
            htmlDocument.WriteLine("<meta name=" + Chr(34) + "viewport" + Chr(34) + " content=" + Chr(34) + "width=device-width, initial-scale=1.0" + Chr(34) + "/>");
            htmlDocument.WriteLine("<link href=" + Chr(34) + "Content/bootstrap.min.css" + Chr(34) + " rel=" + Chr(34) + "stylesheet" + Chr(34) + "/>");
            htmlDocument.WriteLine("<style>");
            htmlDocument.WriteLine("@media only screen and (min-device-width: 541px) {");
            htmlDocument.WriteLine("        .content {");
            htmlDocument.WriteLine("         width: 540px !important;");
            htmlDocument.WriteLine("        }");
            htmlDocument.WriteLine("    }");
            htmlDocument.WriteLine("    mark.red {");
            htmlDocument.WriteLine("        color: #ff0000;");
            htmlDocument.WriteLine("        background: none;");
            htmlDocument.WriteLine("    }");
            htmlDocument.WriteLine("    .wrapper {");
            htmlDocument.WriteLine("        text-align: center;");
            htmlDocument.WriteLine("    }");
            htmlDocument.WriteLine("</style>");
            htmlDocument.WriteLine("</head>");
            htmlDocument.WriteLine("<body>");
            htmlDocument.WriteLine("<!--[if (gte mso 9)|(IE)]>");
            htmlDocument.WriteLine("      <table width=" + Chr(34) + "540" + Chr(34) + " align=" + Chr(34) + "center" + Chr(34) + " cellpadding=" + Chr(34) + "0" + Chr(34) + " cellspacing=" + Chr(34) + "0" + Chr(34) + " border=" + Chr(34) + "0" + Chr(34) + ">");
            htmlDocument.WriteLine("        <tr>");
            htmlDocument.WriteLine("          <td>");
            htmlDocument.WriteLine("    <![endif]-->");
            htmlDocument.WriteLine("    <table class=" + Chr(34) + "content" + Chr(34) + " align=" + Chr(34) + "center" + Chr(34) + " cellpadding=" + Chr(34) + "0" + Chr(34) + " cellspacing=" + Chr(34) + "0" + Chr(34) + " border=" + Chr(34) + "0" + Chr(34) + " style=" + Chr(34) + "width: 100%; max-width: 540px;" + Chr(34) + ">");
            htmlDocument.WriteLine("        <tr>");
            htmlDocument.WriteLine("            <td>");
            htmlDocument.WriteLine("                <h4 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">Id and Password for " + patient.PatientName + "</h4>");
            htmlDocument.WriteLine("                <h4 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">ID: " + patient.PatientUserId + "</h4>");
            htmlDocument.WriteLine("                <h4 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">Password:  " + patient.PatientPassword + "</h4>");
            htmlDocument.WriteLine("<br />");
            htmlDocument.WriteLine("<br />");           
            htmlDocument.WriteLine("                <h4 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">To login go to http://faccia.com</h4>");                         
            htmlDocument.WriteLine("            </td>");
            htmlDocument.WriteLine("        </tr>");
            htmlDocument.WriteLine("    </table>");
            htmlDocument.WriteLine("<br />");
            htmlDocument.WriteLine("</body>");
            htmlDocument.WriteLine("</html>");
            htmlDocument.Close();

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("myshot@fcaaia.com");
                message.To.Add(new MailAddress(parts[1]));
                message.Subject = "Request for UserName and Password";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = System.IO.File.ReadAllText(@"C:\temp\mail.html");
                smtp.Port = 587;
                smtp.Host = "smtp.siteprotect.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("myshot@fcaaia.com", "@0Hinckley");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception er)
             {
                return Ok(new { status = "Failure", message = er.ToString()});              
             }

            return Ok(new { status = "Success", message = "None" });
        }

        private char Chr(byte src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetChars(new byte[] { src })[0]);
        }

    }
}
