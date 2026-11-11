using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Nexam.Infrastructure;

public sealed class EmailSettings
{
    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FromAddress { get; set; }
    /// <summary>Always copied (BCC) on announcement mails.</summary>
    public string AdminAddress { get; set; }
}

public interface IEmailSender
{
    Task SendAsync(string to, IEnumerable<string> bcc, string subject, string htmlBody);
}

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public SmtpEmailSender(IOptions<EmailSettings> options) => _settings = options.Value;

    public async Task SendAsync(string to, IEnumerable<string> bcc, string subject, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(_settings.UserName) || string.IsNullOrWhiteSpace(_settings.Password))
            throw new InvalidOperationException(
                "Email is not configured. Set Email:UserName and Email:Password (use user-secrets or environment variables).");

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress ?? _settings.UserName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);
        foreach (var address in bcc.Where(a => !string.IsNullOrWhiteSpace(a)))
            message.Bcc.Add(address.Trim());

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };
        await client.SendMailAsync(message);
    }
}
