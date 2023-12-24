using System.ComponentModel.DataAnnotations;

namespace NoteProject.Api.Services.Identity.Models
{
    public class ForgetPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
