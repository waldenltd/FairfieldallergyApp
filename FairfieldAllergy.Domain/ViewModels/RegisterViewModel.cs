using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FairfieldAllergy.Domain.ViewModels
{
    public class RegisterViewModel
    {
        //[EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
         "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage =
         "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[EmailAddress]
        //[Remote(action: "IsEmailInUse", controller:"Account")]
        //[ValidEmailDomain(allowedDomain: "pragimtech.com", 
        //    ErrorMessage ="Email domain must be pragimtech.com")]
        //public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password",
        //    ErrorMessage = "Password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string Sex { get; set; }

        //[Required]
        public string HomePhone { get; set; }
    }
}