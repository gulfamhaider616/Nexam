using Microsoft.Data.SqlClient;

namespace Nexam.DataAccess;

public interface IDbConnectionFactory
{
    /// <summary>Creates a new, closed connection. Dapper opens and closes it per call.</summary>
    SqlConnection Create();
}

/// <summary>
/// Connects using the connection string named by <c>Database:ActiveConnection</c>
/// (default <c>DefaultConnection</c>), looked up under <c>ConnectionStrings</c>.
/// </summary>
public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        var name = configuration["Database:ActiveConnection"];
        if (string.IsNullOrWhiteSpace(name)) name = "DefaultConnection";

        var connectionString = configuration.GetConnectionString(name);
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                $"Connection string '{name}' is not configured. Set ConnectionStrings:{name} " +
                $"(appsettings.{{Environment}}.json, user-secrets or the ConnectionStrings__{name} environment variable).");

        _connectionString = connectionString;
    }

    public SqlConnection Create() => new(_connectionString);
}
