using Microsoft.AspNetCore.Mvc;
using NoteProject.Host.Base.Controllers;
using NoteProject.Host.Models;
using NoteProject.Service.Notes;

namespace NoteProject.Host.Controllers
{
    public class NotesController : BaseMvcControllers
    {
        private INoteQueryService _noteQueryService;

        public NotesController(INoteQueryService noteQueryService)
        {
            _noteQueryService = noteQueryService;
        }

        public async Task<ActionResult> Index()
        {
            var notes = await _noteQueryService.GetNotes();
            var model = NoteViewModel.FromDto(notes);
            return View(model);
        }

        [HttpGet("[controller]/{id:int}")]
        public async Task<ActionResult> Details(long id)
        {
            if (id <= 0)
                return ErrorJsonResult("Login", "Note not found");

            var note = await _noteQueryService.GetNoteById(id);
            var model = NoteViewModel.FromDto(note);
            return View(model);
        }
    }
}
