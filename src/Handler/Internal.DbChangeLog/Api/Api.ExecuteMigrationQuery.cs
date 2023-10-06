using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public async ValueTask ExecuteMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        var migrationQuery = await ReadMigrationQueryAsync(migrationItem, cancellationToken).ConfigureAwait(false);

        var migrationRequest = new DbQuery(migrationQuery)
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        _ = await sqlApi.ExecuteNonQueryAsync(migrationRequest, cancellationToken).ConfigureAwait(false);

        var logInsertRequest = new DbQuery(
            query: DbChangeLogInsertQuery,
            parameters: new(
                new("Id", migrationItem.Id),
                new("Comment", migrationItem.Comment.OrEmpty())))
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        _ = await sqlApi.ExecuteNonQueryAsync(logInsertRequest, cancellationToken).ConfigureAwait(false);
    }

    private ValueTask<string> ReadMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(migrationItem.FilePath))
        {
            return new(string.Empty);
        }

        return new(fileReader.ReadAsync(option.BasePath, migrationItem.FilePath, cancellationToken));
    }
}