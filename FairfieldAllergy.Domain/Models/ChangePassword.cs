using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
    }
}
