using NoteProject.Core.Domain.Medias;
using NoteProject.Core.Domain.Medias.Extensions;
namespace NoteProject.Service.Medias.Models
{
    public class MediaDto
    {
        public long Id { get; private set; }
        public MediaType Type { get; set; }
        public string DisplayName { get; private set; }
        public string RelativeUrl { get; private set; }

        public static MediaDto FromEntity(Media media)
        {
            if (media == null)
                return default;
            return new()
            {
                Id = media.Id,
                Type = media.Type,
                DisplayName = media.DisplayName,
                RelativeUrl = media.GetRelativeUrl()
            };
        }
    }
}
