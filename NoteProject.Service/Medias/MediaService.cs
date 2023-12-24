using Microsoft.AspNetCore.Http;
using NoteProject.Core.Base;
using NoteProject.Core.Configuration;
using NoteProject.Core.Domain.Medias;
using NoteProject.Core.Extensions;
using NoteProject.Data.Base;
using NoteProject.Data.Data;
using NoteProject.Service.Medias.Extensions;
using NoteProject.Service.Storage;
using TwentyTwenty.Storage;

namespace NoteProject.Service.Medias
{
    public class MediaService : IMediaService
    {
        private readonly string _localGalleryUrl;

        private readonly IRepository<Media, ApplicationDbContext> _repository;
        private readonly IStorageProvider _storageProvider;

        public MediaService(
            IRepository<Media, ApplicationDbContext> repository,
            IStorageServiceFactory _storageServiceFactory)
        {
            _localGalleryUrl = StorageSettings.Instance.LocalMediaUrl.StartsWith("/") 
                ? StorageSettings.Instance.LocalMediaUrl.Remove(0, 1) 
                : StorageSettings.Instance.LocalMediaUrl;

            _repository = repository;
            _storageProvider = _storageServiceFactory.GetProvider();
        }

        public async Task<CommandResult<Media>> CreateAndSaveMediaEntityInstanceByFormFile(IFormFile file, MediaCategory category)
        {
            var (media, _, _) = await CreateMediaEntityInstanceByStream(file.OpenReadStream(), file.FileName, category, file.ContentType);

            var (isSuccess, errorMessage) = await SaveMediaEntity(media);

            return new(media, isSuccess, errorMessage);
        }

        public async Task<CommandResult<Media>> CreateMediaEntityInstanceByStream(Stream fileStream, string fileName, MediaCategory category, string contentType)
        {
            var isFileSaved = false;

            var fileInfo = new FileInfo(fileName);
            var newFileName = GenerateRandomFileName(fileInfo.Extension);
            var media = new Media()
            {
                Type = GetMediaType(contentType),
                Category = category,
                FileName = newFileName,
                DisplayName = fileInfo.Name,
                Directory = $"{category}/{newFileName}".ConvertToAppropriateDirectorySeperator(),
                ContentType = contentType,
                FileSize = fileStream.Length
            };

            try
            {
                await media.SaveAttachedFile(_storageProvider, fileStream, _localGalleryUrl);

                isFileSaved = true;

                return new(media);
            }
            catch (Exception)
            {
                if (isFileSaved)
                    await media.DeleteAttachedFile(_storageProvider, _localGalleryUrl);

                throw;
            }
        }

        public async Task<CommandResult> RemoveByIdAsync(long mediaId)
        {
            var media = await GetByIdAsync(mediaId);
            if (media is null)
                return new();

            await media.DeleteAttachedFile(_storageProvider, _localGalleryUrl);

            await _repository.RemoveAndSaveAsync(media);

            return new();
        }
        
        private async Task<Media> GetByIdAsync(long id) => await _repository.FindAsync(id);

        private static MediaType GetMediaType(string contentType)
        {
            if (contentType.StartsWith("image"))
                return MediaType.Image;
            else if (contentType.StartsWith("video"))
                return MediaType.Video;
            else
                return MediaType.File;
        }

        private async Task<CommandResult> SaveMediaEntity(Media media)
        {
            // Save the media meta data to DB
            await _repository.AddAndSaveAsync(media);

            return new();
        }

        private string GenerateRandomFileName(string fileExtension)
        {
            fileExtension = fileExtension.StartsWith('.') ? fileExtension : $".{fileExtension}";

            return Guid.NewGuid() + fileExtension;
        }
    }
}
