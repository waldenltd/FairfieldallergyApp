using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class Appointment
    {
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentDescription { get; set; }
        public int SlotID { get; set; }
        public int Location { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
