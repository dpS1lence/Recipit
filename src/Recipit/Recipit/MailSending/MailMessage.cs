namespace Recipit.MailSending
{
    public record MailMessage
    {
        public string To { get; init; } = default!;
        public string Subject { get; init; } = default!;
        public string Content { get; init; } = default!;
    }
}
