using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain
{
    public class FairfieldIdentityUser : IdentityUser
    {
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string HomePhone { get; set; }

   }
}

