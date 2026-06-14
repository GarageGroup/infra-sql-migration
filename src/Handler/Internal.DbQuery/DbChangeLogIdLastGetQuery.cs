using System;

namespace GarageGroup.Infra;

internal sealed record class DbChangeLogIdLastGetQuery : IDbQuery
{
    private const string TransactSqlQuery
        =
        "SELECT TOP 1 Id From [_ChangeLogHistory] ORDER BY CreationTime DESC;";

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery(SqlDialect dialect)
    {
        if (dialect is SqlDialect.TransactSql)
        {
            return TransactSqlQuery;
        }

        throw new NotSupportedException($"Dialect '{dialect}' is not supported for {nameof(DbChangeLogIdLastGetQuery)}.");
    }

    public FlatArray<DbParameter> GetParameters()
        =>
        default;
}