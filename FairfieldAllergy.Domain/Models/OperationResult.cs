using System;
using System.Collections.Generic;

namespace FairfieldAllergy.Domain.Models
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> MessageList { get; private set; }
        public string ResultFromServerCall { get; set; }
        public string ErrorMessage { get; set; }
        public List<PrintDates> PrintDates { get; set; }
        public List<AllergyPatientAppointments> AllergyPatientAppointments { get; set; }
        public List<OpenAppointment> Appointments { get; set; }
        public List<AppointmentSlots> AppointmentSlots { get; set; }
        public List<Appointment> ExistingAppointments { get; set; }
        public UserInformation UserInformation { get; set; }
        public List<Patient> Patient { get; set; }
        public Patient PatientCredentials { get; set; }
        public int CurrentId { get; set; }

        public OperationResult()
        {
            MessageList = new List<string>();
            Success = true;
        }

        public void AddMessage(string message)
        {

            MessageList.Add(message);

        }
    }
}
