using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public string PatientUserId { get; set; }
        public string PatientPassword { get; set; }
        public int PatientLocation { get; set; }
        public int PatientWebId { get; set; }
    }
}
