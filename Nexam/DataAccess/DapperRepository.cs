using System.Data;
using Dapper;

namespace Nexam.DataAccess;

/// <summary>
/// Base class for the Dapper repositories. Wraps the two kinds of database calls the app makes:
/// stored procedures (the *Proc* helpers) and raw SQL (the *Sql* helpers).
/// </summary>
public abstract class DapperRepository
{
    protected readonly IDbConnectionFactory Factory;

    protected DapperRepository(IDbConnectionFactory factory) => Factory = factory;

    // ---------- stored procedures ----------

    protected async Task<List<T>> QueryProcAsync<T>(string procedure, object param = null)
    {
        using var connection = Factory.Create();
        var rows = await connection.QueryAsync<T>(procedure, param, commandType: CommandType.StoredProcedure);
        return rows.AsList();
    }

    protected async Task<T> QueryFirstProcAsync<T>(string procedure, object param = null)
    {
        using var connection = Factory.Create();
        return await connection.QueryFirstOrDefaultAsync<T>(procedure, param, commandType: CommandType.StoredProcedure);
    }

    protected async Task<int> ExecuteProcAsync(string procedure, object param = null)
    {
        using var connection = Factory.Create();
        return await connection.ExecuteAsync(procedure, param, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    /// Runs a "save" procedure and reports success the way the app always has:
    /// the procedure returns the inserted/updated row (OUTPUT INSERTED...), so any row means success.
    /// </summary>
    protected async Task<bool> ProcReturnsRowAsync(string procedure, object param = null)
    {
        using var connection = Factory.Create();
        var rows = await connection.QueryAsync(procedure, param, commandType: CommandType.StoredProcedure);
        return rows.Any();
    }

    // ---------- raw SQL ----------

    protected async Task<List<T>> QuerySqlAsync<T>(string sql, object param = null)
    {
        using var connection = Factory.Create();
        var rows = await connection.QueryAsync<T>(sql, param);
        return rows.AsList();
    }

    protected async Task<T> QueryFirstSqlAsync<T>(string sql, object param = null)
    {
        using var connection = Factory.Create();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    protected async Task<int> ExecuteSqlAsync(string sql, object param = null)
    {
        using var connection = Factory.Create();
        return await connection.ExecuteAsync(sql, param);
    }
}
