using Microsoft.AspNetCore.Http;
using NoteProject.Core.Base;
using NoteProject.Core.Domain.Medias;
using NoteProject.Core.Domain.Notes;
using NoteProject.Data.Base;
using NoteProject.Data.Data;
using NoteProject.Service.Medias;

namespace NoteProject.Service.Notes
{
    public class NoteCommandService : INoteCommandService
    {
        private readonly IRepository<Note, ApplicationDbContext> _noteRepository;
        private readonly IMediaService _mediaService;

        public NoteCommandService(
            IRepository<Note, ApplicationDbContext> noteRepository,
            IMediaService mediaService)
        {
            _noteRepository = noteRepository;
            _mediaService = mediaService;
        }

        private async Task<Note> GetNoteById(long id) => await _noteRepository.FindAsync(id);

        public async Task<CommandResult> CreateNote(string name, string content, IFormFile image = default)
        {
            var note = new Note(name, content);

            await _noteRepository.AddAndSaveAsync(note);

            if (image != null)
            {
                await UpdateNoteImage(note.Id, image);
            }

            return new();
        }

        public async Task<bool> DeleteNote(long NoteId)
        {
            var note = await GetNoteById(NoteId) ?? throw new Exception("Note not found");

            var noteImage = note.Image;
            if (noteImage != null)
            {
                var result = await _mediaService.RemoveByIdAsync(noteImage.Id);
                if(!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                } 
            }

            var affRows = await _noteRepository.RemoveAndSaveAsync(note);
            if (affRows <= 0)
            {
                return false;
            }
            return true;
        }

        public async Task<CommandResult> UpdateNote(long id, string name, string content, IFormFile image = default)
        {
            var note = await GetNoteById(id) ?? throw new Exception("Note not found");

            note.SetName(name);
            note.SetContent(content);
            
            await _noteRepository.ModifyAndSaveAsync(note);

            if (image != null)
            {
                await UpdateNoteImage(note.Id, image);
            }
            return new();
        }

        public async Task<CommandResult> UpdateNoteImage(long noteId, IFormFile image)
        {
            if (image is null)
                throw new Exception("Image Not Found");

            var note = await GetNoteById(noteId) ?? throw new Exception("Note not found");

            var oldImageId = note.Image?.Id;
            var result = await _mediaService.CreateAndSaveMediaEntityInstanceByFormFile(image, MediaCategory.NoteImage);
            if (!result.IsSuccess)
            {
                throw new Exception(result.ErrorMessage);
            }
            note.SetImage(result.Result);

            if (oldImageId != null)
            {
                await _mediaService.RemoveByIdAsync(oldImageId.Value);
            }

            await _noteRepository.ModifyAndSaveAsync(note);

            return new();
        }
    }
}
