using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FairfieldAllergy.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FairfieldAllergy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatesController : ControllerBase
    {

        // GET: api/Dates
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            OperationResult operationResult = new OperationResult();

            operationResult.PrintDates = new List<PrintDates>();
            DateTime now = DateTime.Now;
            int j = 1;

            for (int i = 0; i > -40; i--)
            {
                //2/2/
                var printDates = new PrintDates();
                printDates.Id = j;
                j++;
                printDates.PrintDate = now.AddDays(i).ToShortDateString().Replace("/","-");
                operationResult.PrintDates.Add(printDates);
            }

            operationResult.Success = true;

            if (operationResult.Success)
            {
                return Ok((IEnumerable<PrintDates>)operationResult.PrintDates);
            }
            else
            {
                return Content(HttpStatusCode.NotFound.ToString(), operationResult.ErrorMessage);
            }
        }
    }
}
