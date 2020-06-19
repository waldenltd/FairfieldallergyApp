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
    public class Broadcast : ControllerBase
    {
        // GET: api/Broadcast
        [HttpGet("{parametersString}", Name = "GetBroadcast")]
        public IActionResult Get(string parametersString)
        {
            string[] parts = parametersString.Split('~');

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            List<BroadcastMessage> broadcastMessage = fairfieldAllergeryRepository.GetBroadcastMessage(parts[0], parts[1]);

            return Ok(broadcastMessage);
        }

        // POST: api/Broadcast
        [HttpPost(Name = "AddBroadcast")]
        public IActionResult Post([FromBody] BroadcastMessage broadcastMessage)
        {
            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            fairfieldAllergeryRepository.AddBroadcastMessage(broadcastMessage);

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

