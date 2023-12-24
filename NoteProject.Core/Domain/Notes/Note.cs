using NoteProject.Core.Base;
using NoteProject.Core.Domain.Medias;

namespace NoteProject.Core.Domain.Notes;

public class Note : SoftDeleteAuditableBaseEntity
{
    protected Note() { }
    public Note(string name, string content)
    {
        SetName(name);
        SetContent(content);
    }

    public string Name { get; private set; }
    public void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException("Name can not be empty!");
        }
        Name = name;
    }

    public string Content { get; private set; }
    public void SetContent(string content)
    {
        if (string.IsNullOrEmpty (content))
        {
            throw new ArgumentNullException("Content can not be empty!");
        }
        Content = content;
    }

    public Media? Image { get; private set; }
    public void SetImage(Media image)
    {
        if (image != null)
        {
            Image = image;
        }
    }
}
