using Microsoft.EntityFrameworkCore;
using NoteProject.Core.Domain.Notes;
using NoteProject.Data.Base;
using NoteProject.Data.Data;
using NoteProject.Service.Notes.Models;

namespace NoteProject.Service.Notes;

public class NoteQueryService : INoteQueryService
{
    private readonly IRepository<Note, ApplicationDbContext> _noteRepository;

    public NoteQueryService(
        IRepository<Note, ApplicationDbContext> noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<NoteDto> GetNoteById(long noteId)
    {
        var note = await _noteRepository.Query(e => e.Id == noteId).AsNoTracking().FirstOrDefaultAsync();
        
        if (note == null)
            return default;

        return NoteDto.FromEntity(note);
    }

    public async Task<IEnumerable<NoteDto>> GetNotes()
    {
        var notes = await _noteRepository.QueryAll().AsNoTracking().ToListAsync();

        if (!notes.Any())
            return Enumerable.Empty<NoteDto>();

        var noteDtos = new List<NoteDto>();
        foreach (var note in notes)
        {
            noteDtos.Add(NoteDto.FromEntity(note));
        }
        return noteDtos;
    }
}
