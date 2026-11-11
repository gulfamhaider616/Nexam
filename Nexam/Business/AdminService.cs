using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;

namespace Nexam.Business;

public sealed class AdminService(IAdminRepository admins) : IAdminService
{
    public async Task<string> VerifyAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;
        var name = await admins.VerifyAdminAsync(email, password);
        return string.IsNullOrEmpty(name) ? null : name;
    }
}
