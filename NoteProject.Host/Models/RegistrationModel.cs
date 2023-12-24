using NoteProject.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace NoteProject.Host.Models
{
    public class RegistrationModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegularExpressions.ALPHABETS_AND_SPACE_ONLY)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(RegularExpressions.ALPHABETS_AND_SPACE_ONLY)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
