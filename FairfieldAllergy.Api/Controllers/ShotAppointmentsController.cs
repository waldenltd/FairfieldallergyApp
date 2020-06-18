using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FairfieldAllergy.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShotAppointmentsController : ControllerBase
    {
        // GET: api/PatientShotAppointments/5
        [HttpGet("{parametersString}", Name = "Get")]
        public IActionResult Get(string parametersString)
        {
            string[] parameters = parametersString.Split('~');

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();


            //var user = await fairfieldAllergeryRepository.GetPatientAllergyAppointments(parameters.AllergyShotDate, parameters.Location);

            operationResult = fairfieldAllergeryRepository.GetPatientAllergyAppointments(parameters[0], parameters[1]);

            if (operationResult.Success)
            {
                return Ok((IEnumerable<AllergyPatientAppointments>)operationResult.AllergyPatientAppointments);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }

        }
    }
}
