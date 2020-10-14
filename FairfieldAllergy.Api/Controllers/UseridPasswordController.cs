using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class UseridPasswordController : ControllerBase
    {

        // GET: api/UseridPassword
        [HttpGet("{parametersString}", Name = "GetUserIdPassword")]
        public IActionResult Get(string parametersString)
        {
            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();
            
            operationResult = fairfieldAllergeryRepository.GetUserIdAndPassword(parametersString);

            if (operationResult.Success)
            {
                //return Ok(operationResult.PatientCredentials);
                return Ok(operationResult.UserInformation);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }
    }
}
