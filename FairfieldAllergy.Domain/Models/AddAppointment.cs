using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class AddAppointment
    {
        public string AppointmentTime { get; set; }
        public string AppointmentDescription { get; set; }
        public string SlotID { get; set; }
        public string Location { get; set; }
        public string ScheduleID { get; set; }
    }
}
