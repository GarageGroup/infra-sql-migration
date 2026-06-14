using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed record class DbChangeLogInsertQuery : IDbQuery
{
    private const string TransactSqlQuery
        =
        "INSERT INTO [_ChangeLogHistory](Id, Comment) VALUES (@Id, @Comment);";

    private readonly string migrationId;

    private readonly string comment;

    public DbChangeLogInsertQuery(string migrationId, [AllowNull] string comment)
    {
        this.migrationId = migrationId.OrEmpty();
        this.comment = comment.OrEmpty();
    }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery(SqlDialect dialect)
    {
        if (dialect is SqlDialect.TransactSql)
        {
            return TransactSqlQuery;
        }

        throw new NotSupportedException($"Dialect '{dialect}' is not supported for {nameof(DbChangeLogInsertQuery)}.");
    }

    public FlatArray<DbParameter> GetParameters()
        =>
        [
            new("Id", migrationId),
            new("Comment", comment)
        ];
}