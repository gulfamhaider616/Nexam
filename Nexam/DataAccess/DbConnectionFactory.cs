using Microsoft.Data.SqlClient;

namespace Nexam.DataAccess;

public interface IDbConnectionFactory
{
    /// <summary>Creates a new, closed connection. Dapper opens and closes it per call.</summary>
    SqlConnection Create();
}

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public SqlConnection Create() => new(_connectionString);
}
