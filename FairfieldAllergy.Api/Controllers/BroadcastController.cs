using System.Collections.Generic;
using System.Net;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;

namespace FairfieldAllergy.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Broadcast : ControllerBase
    {
        // GET: api/Broadcast
        [HttpGet("{parametersString}", Name = "GetBroadcast")]
        public IActionResult Get(string parametersString)
        {
            string[] parts = parametersString.Split('~');

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();


            Patient patient = fairfieldAllergeryRepository.GetPatientNameForEmail(parts[0]);


            var failedEmails = new List<string>();
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Fairfiled Allergy","myshot@fcaaia.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(patient.PatientName, parts[1]);
            message.To.Add(to);

            message.Subject = "User Id and Password for My Shot Application";

            TextWriter htmlDocument = new StreamWriter("C:\\temp\\email.txt");

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
            htmlDocument.WriteLine("         width: 640px !important;");
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
            //htmlDocument.WriteLine("                <a href=" + Chr(34) + "https://www.center4ortho.com/" + Chr(34) + "><img src=" + Chr(34) + "http://52.186.125.33/RemindersWeb/img/cfo.png" + Chr(34) + " alt=" + Chr(34) + "Center For Orthopedic" + Chr(34) + "></a>");
            htmlDocument.WriteLine("                <h2 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">THIS IS AN AUTOMATED EMAIL FROM FAIRFIELD ALLERGY" + "</h2>");            
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <h3 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">Your User ID is " + patient.PatientUserId + "</h3>");
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <h3 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">Your Password is " + patient.PatientPassword + "</h3>");
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <br />");
            htmlDocument.WriteLine("                <br />");            
            htmlDocument.WriteLine("                <h2 style=" + Chr(34) + "font-weight: bold;" + Chr(34) + ">THIS IS AN UNATTENDED EMAIL PLEASE DO NOT REPLY</h2>");            
            //htmlDocument.WriteLine("                <h4 align=CENTER><a href=" + Chr(34) + "?appointmentId=" + "&appointmentDate=" + Chr(34) + ">Confirm Your Appointment</a></h4>");
            htmlDocument.WriteLine("            </td>");
            htmlDocument.WriteLine("        </tr>");
            htmlDocument.WriteLine("    </table>");
            htmlDocument.WriteLine("<br />");
            htmlDocument.WriteLine("</body>");
            htmlDocument.WriteLine("</html>");
            htmlDocument.Close();
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = System.IO.File.ReadAllText("C:\\temp\\email.txt");

            /*
            bodyBuilder.TextBody = "Your User ID is "
            + patient.PatientUserId
            + " and the password is "
            + patient.PatientPassword
            + "\r\n"
            + "\r\n"
            + "This is an unattended email please do not reply ";
*/
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            //  client.Connect("smtp.siteprotect.com", 587, true);
            client.Connect("smtp.siteprotect.com", 587, MailKit.Security.SecureSocketOptions.Auto);

            client.Authenticate("myshot@fcaaia.com", "@0Luscomb");

            try
            {
                client.Send(message);
            }
            catch (SmtpCommandException ex)
            {
                //  Console.WriteLine ("Error sending message: {0}", ex.Message);
                // Console.WriteLine ("\tStatusCode: {0}", ex.StatusCode);

                switch (ex.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Console.WriteLine("\tRecipient not accepted: {0}", ex.Mailbox);
                        break;
                    case SmtpErrorCode.SenderNotAccepted:
                        Console.WriteLine("\tSender not accepted: {0}", ex.Mailbox);
                        break;
                    case SmtpErrorCode.MessageNotAccepted:
                        Console.WriteLine("\tMessage not accepted.");
                        break;
                }
            }

            client.Disconnect(true);
            client.Dispose();

            return Ok(new { status = "Success" });

        }

        // POST: api/Broadcast
        [HttpPost(Name = "AddBroadcast")]
        public IActionResult Post([FromBody] Broadcast appointment)
        {
            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

           // operationResult = fairfieldAllergeryRepository.AddAppointment(words[0], words[1] + words[2], appointment.Location.ToString(), appointment.UserId.ToString(), appointment.SlotID.ToString());

            if (operationResult.Success)
            {
                return Ok(new { status = "Success" });
            }
            else
            {
                return Ok(new { status = "Failure" });
            }
        }


        private char Chr(byte src)
        {
            return (System.Text.Encoding.GetEncoding("iso-8859-1").GetChars(new byte[] { src })[0]);
        }

    }
}

