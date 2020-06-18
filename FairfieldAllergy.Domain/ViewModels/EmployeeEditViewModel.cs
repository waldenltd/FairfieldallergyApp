using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairfieldAllergy.Domain.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
