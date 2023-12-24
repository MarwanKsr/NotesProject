using NoteProject.Core.Configuration;
using NoteProject.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteProject.Core.Domain.Medias.Extensions
{
    public static class MediaExtensions
    {
        public static string GetRelativeUrl(this Media media)
        {
            var storageSettings = StorageSettings.Instance;

            var mediaUrl = storageSettings.LocalMediaUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase) 
                ? storageSettings.LocalMediaUrl.Remove(0, 1) 
                : storageSettings.LocalMediaUrl;

            mediaUrl = mediaUrl.Replace("wwwroot/", "");
            mediaUrl = mediaUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? mediaUrl[..^1] : mediaUrl;

            return media is null ? "" : $"/{mediaUrl}/{media.Category}/{media.FileName}".ToUrl();
        }
    }
}
