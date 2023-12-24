
namespace NoteProject.Service.MessageSender
{
    public interface IEmailMessageSender
    {
        Task<bool> SendMessageAsync(string to, string subject, string body);
    }
}
