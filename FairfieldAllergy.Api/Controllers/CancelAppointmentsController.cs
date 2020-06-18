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
    public class CancelAppointmentsController : ControllerBase
    {
        // GET: api/CancelAppointments/5
        [HttpGet("{parametersString}", Name = "CancelAppointments")]
        public IActionResult Get(string parametersString)
        {
             string[] parameters = parametersString.Split('~');

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.GetCurrentAppointments(parameters[0]);

            if (operationResult.Success)
            {
                return Ok((IEnumerable<Appointment>)operationResult.ExistingAppointments);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }

        // DELETE: api/CancelAppointments/5
        [HttpDelete("{parametersString}", Name = "DeleteAppointments")]
        public IActionResult Delete(string parametersString)
        {
            string s1 = parametersString.ToString();

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.DeleteAppointment(parametersString);

            if (operationResult.Success)
            {
                return Ok(new { status = "Success" });
            }
            else
            {
                return Ok(new { status = "Failure" });
            }
        }
    }
}
