using System.ComponentModel.DataAnnotations;

namespace NoteProject.Host.Areas.User.Models.Notes
{
    public class NoteCreateModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public IFormFile? Image { get; set; }
    }
}
