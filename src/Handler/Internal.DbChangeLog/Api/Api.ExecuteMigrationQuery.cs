using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DbChangeLogApi
{
    public async ValueTask ExecuteMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        var migrationQuery = await ReadMigrationQueryAsync(migrationItem, cancellationToken).ConfigureAwait(false);

        var sqlRequest = new DbQuery(
            query: BuildMigrationTransactionQuery(migrationQuery),
            parameters: new FlatArray<DbParameter>(
                new("Id", migrationItem.Id),
                new("Comment", migrationItem.Comment ?? string.Empty)));

        _ = await sqlApi.ExecuteNonQueryAsync(sqlRequest, cancellationToken).ConfigureAwait(false);
    }

    private ValueTask<string> ReadMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(migrationItem.FilePath))
        {
            return new(string.Empty);
        }

        return new(fileReader.ReadAsync(option.BasePath, migrationItem.FilePath, cancellationToken));
    }

    private static string BuildMigrationTransactionQuery(string migrationQuery)
    {
        if (string.IsNullOrEmpty(migrationQuery))
        {
            return DbChangeLogInsertQuery;
        }

        return $"""
            BEGIN TRANSACTION;
                {migrationQuery}
                {DbChangeLogInsertQuery}
            COMMIT;
            """;
    }
}