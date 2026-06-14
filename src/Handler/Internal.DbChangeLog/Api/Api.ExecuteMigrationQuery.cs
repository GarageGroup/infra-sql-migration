using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public async Task ExecuteMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        var migrationQuery = await ReadMigrationQueryAsync(migrationItem, cancellationToken).ConfigureAwait(false);

        var migrationRequest = new DbQuery(migrationQuery)
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        _ = await sqlApi.ExecuteNonQueryAsync(migrationRequest, cancellationToken).ConfigureAwait(false);

        var logInsertRequest = new DbChangeLogInsertQuery(
            migrationId: migrationItem.Id,
            comment: migrationItem.Comment)
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        _ = await sqlApi.ExecuteNonQueryAsync(logInsertRequest, cancellationToken).ConfigureAwait(false);
    }

    private Task<string> ReadMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(migrationItem.FilePath))
        {
            return Task.FromResult(string.Empty);
        }

        return fileReader.ReadAsync(option.BasePath, migrationItem.FilePath, cancellationToken);
    }
}