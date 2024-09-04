using Solid.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Auth.Models
{
    public class RequestModels
    {

    }
    public class RegisterRequestModel
    {

        public string UserName { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 50 characters.")]

        [Required(ErrorMessage = "First Name Required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required"), EmailAddress(ErrorMessage = "Not a valid email")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters.")]
        [ContainsLetter]
        [ContainsDigit]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords Doesn't match")]
        public string ConfirmPassword { get; set; }

        public int? OrganisationID { get; set; }
    }
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "User Name Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters.")]
        [ContainsLetter]
        [ContainsDigit]
        public string Password { get; set; }
    }
}
