using FluentEmail.Core;

namespace NoteProject.Service.MessageSender
{
    public class EmailMessageSender : IEmailMessageSender
    {
        const string DEFAULT_TAG = "primary";
        private readonly IFluentEmailFactory _fluentEmailFactory;

        public EmailMessageSender(IFluentEmailFactory fluentEmailFactory)
        {
            _fluentEmailFactory = fluentEmailFactory;
        }

        private IFluentEmail CreatEmail() => _fluentEmailFactory.Create();

        public async Task<bool> SendMessageAsync(string to, string subject, string body)
        {
            var email = CreatEmail()
            .To(to)
            .Subject(subject)
            .Body(body, true)
            .Tag(DEFAULT_TAG);

            var emailResponse = await email.SendAsync();
            return emailResponse.Successful;
        }
    }
}
