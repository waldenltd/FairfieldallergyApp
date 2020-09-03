using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FairfieldAllergy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        // GET: api/Appointments/5
        [HttpGet("{parametersString}", Name = "GetAppointments")]
        public IActionResult Get(string parametersString)
        {
            string[] parameters = parametersString.Split('~');
            string parameterTest = parameters[0].Substring(0, 10);

            DateTime date = Convert.ToDateTime(parameterTest);


            string parameters2 = parameters[0].Substring(0, 15);
            string monthString = parameters2.Substring(4, 3);
            string dayMonth = parameters2.Substring(7, 8).Replace(" ", "-");
            string newDate = string.Empty;

            switch (monthString)
            {
                case "Jan":
                    newDate = "01" + dayMonth;
                    break;
                case "Feb":
                    newDate = "02" + dayMonth;
                    break;
                case "Mar":
                    newDate = "03" + dayMonth;
                    break;
                case "Apr":
                    newDate = "04" + dayMonth;
                    break;
                case "May":
                    newDate = "05" + dayMonth;
                    break;
                case "Jun":
                    newDate = "06" + dayMonth;
                    break;
                case "Jul":
                    newDate = "07" + dayMonth;
                    break;
                case "Aug":
                    newDate = "08" + dayMonth;
                    break;
                case "Sep":
                    newDate = "09" + dayMonth;
                    break;
                case "Oct":
                    newDate = "10" + dayMonth;
                    break;
                case "Nov":
                    newDate = "11" + dayMonth;
                    break;
                case "Dec":
                    newDate = "12" + dayMonth;
                    break;
                default:
                    // code block
                    break;
            }

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.GetListOfAppointment(date.ToShortDateString() , parameters[1]);

            if (operationResult.Success)
            {
                return Ok((IEnumerable<OpenAppointment>)operationResult.Appointments);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }
    }
}
