using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Configuration;
using FairfieldAllergy.Data;
using FairfieldAllergy.Domain.Models;
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
        public async Task<IActionResult> Get(string parametersString)
        {
            string[] parameters = parametersString.Split('~');

            string arguements = parameters[0] + " " + parameters[1];

            OperationResult operationResult = new OperationResult();

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();            

            operationResult = fairfieldAllergeryRepository.GetPatientAllergyAppointments(parameters[0], parameters[1]);

            //using (var process = new Process())
            //{
            //    process.StartInfo.FileName = @"C:\waldenltd\FairfieldAllergyApp\ProcessReport\bin\Debug\ProcessReport.exe"; // relative path. absolute path works too.
            //    process.StartInfo.Arguments = arguements;
            //    process.StartInfo.CreateNoWindow = true;
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.RedirectStandardOutput = true;
            //    process.StartInfo.RedirectStandardError = true;
            //    process.Start();
            //    var exited = process.WaitForExit(1000 * 3); 
            //}

            var url = ConfigurationValues.PrintScheduleUrl + parametersString;

            using var client = new HttpClient();

            var response = await client.GetStringAsync(url);

            string[] responseParts = response.Split('~');

            if (operationResult.Success && responseParts[0] == "OK")
            {
                return Ok(new { status = "Success", name = responseParts[1] });
            }
            else
            {
                return Ok(new { status = "No Records", error = operationResult.ErrorMessage});
            }
        }
    }
}
