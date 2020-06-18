using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class UserInformation
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string HomePhone { get; set; }
        public int Location { get; set; }
        public string Status { get; set; }
    }
}
