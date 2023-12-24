using Microsoft.AspNetCore.Mvc;
using NoteProject.Service.Notes.Models;

namespace NoteProject.Host.Models
{
    public class NoteViewModel
    {
        [HiddenInput]
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Content { get; private set; }
        public string ImageUrl { get; private set; }

        public static NoteViewModel FromDto(NoteDto note)
        {
            return new()
            {
                Id = note.Id,
                Name = note.Name,
                Content = note.Content,
                ImageUrl = note.Image.RelativeUrl,
            };
        }

        public static IEnumerable<NoteViewModel> FromDto(IEnumerable<NoteDto> notes)
        {
            foreach (var note in notes)
            {
                yield return new()
                {
                    Id = note.Id,
                    Name = note.Name,
                    Content = note.Content,
                    ImageUrl = note.Image?.RelativeUrl,
                };
            }
        }
    }
}
