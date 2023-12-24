using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NoteProject.Host.Models
{
    public class SetNewPasswordModel
    {
        [HiddenInput]
        public string Email { get; set; }

        [HiddenInput]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
