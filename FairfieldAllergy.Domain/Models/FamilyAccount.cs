using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class FamilyAccount
    {
        public int Id { get; set; }
        public int MainAccount { get; set; }
        public int AddedAccount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
