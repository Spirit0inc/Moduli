using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryApp.Models;

namespace LibraryApp.Validators
{
    public class UserValidator
    {
        public List<ValidationResult> Validate(User user)
        {
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(user, context, results, true);
            return results;
        }
    }
}