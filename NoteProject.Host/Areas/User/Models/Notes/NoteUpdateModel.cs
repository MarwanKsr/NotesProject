using Microsoft.AspNetCore.Mvc;

namespace NoteProject.Host.Areas.User.Models.Notes
{
    public class NoteUpdateModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
    }
}
