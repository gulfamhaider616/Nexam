using Nexam.Business.Interfaces;
using Nexam.Infrastructure;

namespace Nexam.Business;

/// <summary>One sign-in for everyone: an administrator account wins, otherwise a regular user account.</summary>
public sealed class AccountService(IAdminService admins, IUserService users) : IAccountService
{
    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;
        email = email.Trim();

        var adminName = await admins.VerifyAsync(email, password);
        if (adminName is not null)
            return new LoginResult(AppRoles.Admin, adminName, null);

        var user = await users.VerifyAsync(email, password);
        return user is null ? null : new LoginResult(AppRoles.User, user.FName, user.Id);
    }
}
