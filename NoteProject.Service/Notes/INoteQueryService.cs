using NoteProject.Service.Notes.Models;

namespace NoteProject.Service.Notes
{
    public interface INoteQueryService
    {
        Task<IEnumerable<NoteDto>> GetNotes();
        Task<NoteDto> GetNoteById(long noteId);
    }
}
