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
    public class MakeAppointmentController : ControllerBase
    {

        // POST: api/MakeAppointment
        [HttpPost(Name = "MakeAppointment")]
        public IActionResult Post([FromBody] Appointment appointment)
        {
            OperationResult operationResult = new OperationResult();
            string appointmentString = string.Empty;

            appointmentString = appointment.AppointmentDescription;
            appointmentString = appointmentString.Replace("Appt on ", "");
            appointmentString = appointmentString.Replace("at ", "");
            string[] words = appointmentString.Split(' ');

            FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

            operationResult = fairfieldAllergeryRepository.AddAppointment(words[3], words[0] + words[1], appointment.Location.ToString(), appointment.UserId.ToString(), appointment.SlotID.ToString());

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
