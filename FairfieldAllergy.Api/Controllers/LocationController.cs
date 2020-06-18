using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FairfieldAllergy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        // PUT: api/Location/5
        [HttpPut("{id}")]
        public IActionResult Put(string id)
        {
            OperationResult operationResult = new OperationResult();
            string[] words = id.Split('~');

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.UpdatePatientLocation(words[0], words[1]);

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
