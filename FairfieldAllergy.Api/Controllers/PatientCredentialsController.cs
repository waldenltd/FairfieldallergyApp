using System.Collections.Generic;
using System.Net;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FairfieldAllergy.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientCredentalsController : ControllerBase
    {
        // GET: api/PatientCredentals
        [HttpGet("{parametersString}", Name = "GetPatientCredentials")]
        public IActionResult Get(string parametersString)
        {
            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.GetPatientIdAndPassword(parametersString);

            if (operationResult.Success)
            {
                return Ok(operationResult.PatientCredentials);
                //return Ok((IEnumerable<Patient>)operationResult.PatientCredentials);                
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }

        }

        // GET: api/PatientCredentals
        [HttpPost("{parametersString}", Name = "UpdatePatientCredentials")]
        public IActionResult Post(string parametersString)
        {
            OperationResult operationResult = new OperationResult();

            string[] values = parametersString.Split('~');

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.UpdateUserName(values[0], values[1], values[2]);

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
