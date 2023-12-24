using NoteProject.Core.Domain.Notes;
using NoteProject.Service.Medias.Models;

namespace NoteProject.Service.Notes.Models
{
    public class NoteDto
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Content { get; private set; }
        public MediaDto Image { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime? ModifiedAt { get; private set; }

        public static NoteDto FromEntity(Note note)
        {
            return new()
            {
                Id = note.Id,
                Name = note.Name,
                Content = note.Content,
                Image = MediaDto.FromEntity(note.Image),
                CreatedBy = note.CreatedBy,
                CreatedAt = note.CreatedAt,
                ModifiedBy = note.ModifiedBy,
                ModifiedAt = note.ModifiedAt,
            };
        }
    }
}
