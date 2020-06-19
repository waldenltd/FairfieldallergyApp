using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class BroadcastMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public string Norwalk { get; set; }
        public string Greenwich { get; set; }
        public string Stamford { get; set; }
        public string Ridgefield { get; set; }
    }
}
