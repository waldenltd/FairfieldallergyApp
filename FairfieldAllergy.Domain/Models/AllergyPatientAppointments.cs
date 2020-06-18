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
        //public string Account { get; set; }
        //public string TelephoneNumber { get; set; }
    }
}
