using NoteProject.Core.Base;

namespace NoteProject.Core.Domain.Medias
{
    public class Media : SoftDeleteAuditableBaseEntity
    {
        public MediaType Type { get; set; }

        public MediaCategory Category { get; set; }

        public string FileName { get; set; }

        public string DisplayName { get; set; }

        public string Directory { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }
    }
}
