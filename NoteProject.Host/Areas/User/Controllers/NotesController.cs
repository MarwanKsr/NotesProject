using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteProject.Core.Roles;
using NoteProject.Host.Areas.User.Models.Notes;
using NoteProject.Host.Base.Controllers;
using NoteProject.Service.Notes;

namespace NoteProject.Host.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = Roles.User)]
    public class NotesController : BaseMvcControllers
    {
        private readonly INoteCommandService _noteCommandService;
        private readonly INoteQueryService _noteQueryService;

        public NotesController(
            INoteCommandService noteCommandService,
            INoteQueryService noteQueryService)
        {
            _noteCommandService = noteCommandService;
            _noteQueryService = noteQueryService;
        }

        [HttpGet("[area]/[controller]/Create")]
        public IActionResult Create()
        {
            var createModel = new NoteCreateModel();
            return View(createModel);
        }

        [HttpPost("[area]/[controller]/Create")]
        public async Task<IActionResult> Create(NoteCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return ErrorJsonResult("Notes", message);
            }

            await _noteCommandService.CreateNote(createModel.Name, createModel.Content, createModel.Image);

            return SuccessJsonResult("Notes", "the note has been created successfully");
        }

        [HttpGet("[area]/[controller]/[action]/{id:int}")]
        public async Task<IActionResult> Edit(long id)
        {
            if (id == 0)
            {
                return ErrorJsonResult("Notes", "Note not found");
            }

            var note = await _noteQueryService.GetNoteById(id);
            if (note == null)
                return ErrorJsonResult("Notes", "Note not found");

            var model = new NoteUpdateModel()
            {
                Id = note.Id,
                Name = note.Name,
                Content = note.Content,
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("[area]/[controller]/[action]/{id:int}")]
        public async Task<IActionResult> Edit(int id, NoteUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return ErrorJsonResult("Notes", message);
            }

            await _noteCommandService.UpdateNote(updateModel.Id, updateModel.Name, updateModel.Content, updateModel.Image);

            return SuccessJsonResult("Notes", "the note has been updated successfully");
        }

        [HttpGet("[area]/[controller]/[action]/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return ErrorJsonResult("Notes", "note not found");
            }

            await _noteCommandService.DeleteNote(id);

            return SuccessJsonResult("Notes", "the note has been deleted successfully");
        }
    }
}
