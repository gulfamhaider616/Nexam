using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;

namespace Nexam.DataAccess;

public sealed class AdminRepository : DapperRepository, IAdminRepository
{
    public AdminRepository(IDbConnectionFactory factory) : base(factory) { }

    public async Task<string> VerifyAdminAsync(string email, string password)
    {
        var row = await QueryFirstProcAsync<AdminRow>("VerifyAdmin",
            new { AdminEmail = email, AdminPassword = password });
        return row?.AdminName;
    }
}
