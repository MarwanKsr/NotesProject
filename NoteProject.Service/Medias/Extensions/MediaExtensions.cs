using NoteProject.Core.Domain.Medias;
using NoteProject.Core.Extensions;
using TwentyTwenty.Storage;

namespace NoteProject.Service.Medias.Extensions;

public static class MediaExtensions
{
    public static async Task SaveAttachedFile(this Media media, IStorageProvider storageProvider, Stream stream, string galleryUrl)
    {
        try
        {
            await storageProvider.SaveBlobStreamAsync(galleryUrl.ConvertToAppropriateDirectorySeperator(), media.Directory, stream);
        }
        catch { }
    }

    public static async Task DeleteAttachedFile(this Media media, IStorageProvider storageProvider, string galleryUrl)
    {
        try
        {
            await storageProvider.DeleteBlobAsync(galleryUrl.ConvertToAppropriateDirectorySeperator(), media.Directory);
        }
        catch { }
    }
}
