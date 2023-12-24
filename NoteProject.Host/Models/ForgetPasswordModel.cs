using System.ComponentModel.DataAnnotations;

namespace NoteProject.Host.Models
{
    public class ForgetPasswordModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
    }
}
