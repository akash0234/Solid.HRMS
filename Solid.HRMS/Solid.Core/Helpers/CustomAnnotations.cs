using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solid.Core.Helpers
{
    public class CustomAnnotations
    {
    }
    
    /// <summary>
    /// Contains Letter in string Property
    /// </summary>
    public class ContainsLetterAttribute : ValidationAttribute
    {
        public ContainsLetterAttribute()
        {
            ErrorMessage = "Password must contain at least one letter.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password != null && !Regex.IsMatch(password, "[A-Za-z]"))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
    /// <summary>
    /// Contains Digit in string Property
    /// </summary>
    public class ContainsDigitAttribute : ValidationAttribute
    {
        public ContainsDigitAttribute()
        {
            ErrorMessage = "Password must contain at least one digit.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password != null && !Regex.IsMatch(password, "[0-9]"))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
    /// <summary>
     /// Contains Special in string Property
     /// </summary>
    public class ContainsSpecialCharAttribute : ValidationAttribute
    {
        public ContainsSpecialCharAttribute()
        {
            ErrorMessage = "Password must contain at least one special character.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password != null && !Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
