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
    public class ReviewAppointmentsController : ControllerBase
    {
        // GET: api/ReviewAppointments/5
        [HttpGet("{parametersString}", Name = "ReviewAppointments")]
        public IActionResult Get(string parametersString)
        {
            string[] parameters = parametersString.Split('~');

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.GetExistingAppointments(parameters[0]);

            if (operationResult.Success)
            {
                return Ok((IEnumerable<Appointment>)operationResult.ExistingAppointments);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }
    }
}
