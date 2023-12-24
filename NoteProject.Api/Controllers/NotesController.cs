using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteProject.Api.Models.Notes;
using NoteProject.Core.Base;
using NoteProject.Service.Notes;
using NoteProject.Service.Notes.Models;

namespace NoteProject.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    public class NotesController : ControllerBase
    {
        private readonly INoteQueryService _noteQueryService;
        private readonly INoteCommandService _noteCommandService;

        public NotesController(INoteQueryService noteQueryService, INoteCommandService noteCommandService)
        {
            _noteQueryService = noteQueryService;
            _noteCommandService = noteCommandService;
        }

        [HttpGet("api/v1/[controller]")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<NoteDto>>>> GetNotes()
        {
            var notes = await _noteQueryService.GetNotes();
            return Ok(ApiResponse<IEnumerable<NoteDto>>.Create(notes));
        }

        [HttpPost("api/v1/[controller]/Create")]
        public async Task<ActionResult<ApiResponse>> CreateNote([FromBody] NoteCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return BadRequest(ApiResponse<string>.Failure(message));
            }

             await _noteCommandService.CreateNote(createModel.Name, createModel.Content);

            return Ok(ApiResponse<string>.Create("the note has been created successfully"));
        }

        [HttpPost("api/v1/[controller]/{id:int}/Update")]
        public async Task<ActionResult<ApiResponse>> UpdateNote([FromBody] NoteUpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return BadRequest(ApiResponse<string>.Failure(message));
            }

            await _noteCommandService.UpdateNote(updateModel.Id, updateModel.Name, updateModel.Content);

            return Ok(ApiResponse<string>.Create("the note has been updated successfully"));
        }

        [HttpPost("api/v1/[controller]/{id:int}/UpdateImage")]
        public async Task<ActionResult<ApiResponse>> UpdateNoteImage(long id, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return BadRequest(ApiResponse<string>.Failure(message));
            }

            await _noteCommandService.UpdateNoteImage(id, image);

            return Ok(ApiResponse<string>.Create("Note's image has been updated successfully"));
        }

        [HttpPost("api/v1/[controller]/{id:int}/Delete")]
        public async Task<ActionResult<ApiResponse>> DeleteNote(long id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<string>.Failure("note not found"));
            }

            await _noteCommandService.DeleteNote(id);

            return Ok(ApiResponse<string>.Create("the note has been deleted successfully"));
        }
    }
}
