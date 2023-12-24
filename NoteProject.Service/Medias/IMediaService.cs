using Microsoft.AspNetCore.Http;
using NoteProject.Core.Base;
using NoteProject.Core.Domain.Medias;

namespace NoteProject.Service.Medias
{
    public interface IMediaService
    {
        Task<CommandResult<Media>> CreateMediaEntityInstanceByStream(Stream fileStream, string fileName, MediaCategory category, string contentType);

        Task<CommandResult<Media>> CreateAndSaveMediaEntityInstanceByFormFile(IFormFile file, MediaCategory category);

        Task<CommandResult> RemoveByIdAsync(long mediaId);
    }
}
