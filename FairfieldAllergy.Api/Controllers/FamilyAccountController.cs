using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FairfieldAllergy.Domain;

namespace FairfieldAllergy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyAccountController : ControllerBase
    {

        // GET: api/FamilyAccount/5
        [HttpGet("{parametersString}", Name = "GetFamilyAccounts")]
        public IActionResult Get(int parametersString)
        {
            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.GetFamilyAccount(parametersString);

            if (operationResult.Success)
            {
                return Ok((IEnumerable<Patient>)operationResult.Patient);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }

        // POST: api/FamilyAccount
        [HttpPost(Name = "MakeFamilyAccount")]
        public IActionResult Post([FromBody] FamilyAccount familyAccount)
        {
            OperationResult operationResult = new OperationResult();
            string appointmentString = string.Empty;

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.AddFamilyAccount(familyAccount);

            if (operationResult.Success)
            {
                return Ok(new { status = "Success" });
            }
            else
            {
                return Ok(new { status = operationResult.ErrorMessage });
            }
        }
    }
}
