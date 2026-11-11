using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Nexam.Infrastructure;

public sealed class TwilioSettings
{
    public string AccountSid { get; set; }
    /// <summary>Secret. Keep it in user-secrets or the Twilio__AuthToken environment variable, not in a committed file.</summary>
    public string AuthToken { get; set; }
    /// <summary>The Twilio phone number messages are sent from (E.164, e.g. +12345678900).</summary>
    public string FromNumber { get; set; }
}

public interface ISmsSender
{
    /// <returns>The Twilio message SID.</returns>
    Task<string> SendAsync(string toNumber, string body);
}

public sealed class TwilioSmsSender : ISmsSender
{
    private readonly TwilioSettings _settings;

    public TwilioSmsSender(IOptions<TwilioSettings> options) => _settings = options.Value;

    public async Task<string> SendAsync(string toNumber, string body)
    {
        if (string.IsNullOrWhiteSpace(_settings.AccountSid) || string.IsNullOrWhiteSpace(_settings.AuthToken)
            || string.IsNullOrWhiteSpace(_settings.FromNumber))
            throw new InvalidOperationException(
                "Twilio is not configured. Set Twilio:AccountSid, Twilio:AuthToken and Twilio:FromNumber.");

        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        var message = await MessageResource.CreateAsync(
            body: body,
            from: new PhoneNumber(_settings.FromNumber),
            to: new PhoneNumber(toNumber));
        return message.Sid;
    }
}
