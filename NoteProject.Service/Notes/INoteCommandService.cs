using Microsoft.AspNetCore.Http;
using NoteProject.Core.Base;

namespace NoteProject.Service.Notes
{
    public interface INoteCommandService
    {
        Task<CommandResult> CreateNote(string name, string content, IFormFile image = default);
        Task<CommandResult> UpdateNote(long id, string name, string content, IFormFile image = default);
        Task<CommandResult> UpdateNoteImage(long noteId, IFormFile image);
        Task<bool> DeleteNote(long NoteId);
    }
}
