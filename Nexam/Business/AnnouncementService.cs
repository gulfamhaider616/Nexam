using Microsoft.Extensions.Options;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Infrastructure;

namespace Nexam.Business;

/// <summary>
/// The registration announcement mail that used to live in EmailSenderBA.SendEmail.
/// It is not triggered by any page (it never was); call it from a controller or a scheduled job if needed.
/// </summary>
public sealed class AnnouncementService(IUserRepository users, IEmailSender email, IOptions<EmailSettings> settings)
    : IAnnouncementService
{
    private const string Subject = "Thanks for Registration | Nexam.com";

    private const string Body = """
        Dear User,<br/><br/>

        Thank you so much for your registration to <a href='http://nexam.com' target='_blank'> Nexam.com</a>.<br/><br/>

        <a href='http://nexam.com' target='_blank'>Nexam</a> is 100 % free platform and we'll not charge anything from the users. It's just for the help of those people who want to prepare themselves and looking for a job in Pakistan. Our team is continuously working to add more data in our web platform. We are also requesting you to help us to improve this system.
        <br/><br/>
        We are also going to build a new module in which we allow the users to add their own mcqs according to their subject and earn from our website on the daily basis. This will increase your daily earning.
        <br/><br/>
        Please give us feedback to improve this, we will appreciate you for this act.
        <br/><br/>
        If you have any question, feel free to contact us.
        <br/><br/>
        Thanks,<br/>

        Ahmed Shair<br/><br/>

        CTO <a href='http://nexam.com' target='_blank'>Nexam.com</a>
        """;

    public async Task SendRegistrationAnnouncementAsync()
    {
        var admin = settings.Value.AdminAddress;
        if (string.IsNullOrWhiteSpace(admin))
            throw new InvalidOperationException("Email:AdminAddress is not configured.");

        var recipients = (await users.GetAllAsync()).Select(u => u.Email).Append(admin);
        await email.SendAsync(admin, recipients, Subject, Body);
    }
}
