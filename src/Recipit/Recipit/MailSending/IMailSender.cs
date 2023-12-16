namespace Recipit.MailSending
{
    public interface IMailSender
    {
        Task SendEmailAsync(MailMessage message);

    }
}
