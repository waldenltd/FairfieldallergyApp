using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class AllergyPatientAppointments
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfAppointment { get; set; }
        public string TimeOfAppointment { get; set; }        
        public string DateOfBirth { get; set; }
        public string SlotId { get; set; }
        public string LocationID { get; set; }
        public DateTime SlotDate { get; set; }
        public DateTime SlotTime { get; set; }
        //select A.SlotId as SlotId, A.LocationID, A.AppointmentTypeID, A.NumberSlots, A.SlotDate, A.SlotTime,


        //public string Account { get; set; }
        //public string TelephoneNumber { get; set; }
    }
}
