using System;
using System.Collections.Generic;
using System.Text;

namespace FairfieldAllergy.Domain.Models
{
    public class AppointmentSlots
    {
        public int SlotId { get; set; }
        public int LocationID { get; set; }
        public int AppointmentTypeID { get; set; }
        public int NumberSlots { get; set; }
        public string SlotDate { get; set; }
        public string SlotTime { get; set; }
        public int NewSlotNumber { get; set; }
    }
}
