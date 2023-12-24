namespace NoteProject.Api.Models.Notes
{
    public class NoteCreateModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class NoteUpdateModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
