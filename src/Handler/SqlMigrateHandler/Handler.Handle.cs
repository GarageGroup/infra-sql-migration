using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

partial class SqlMigrateHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(Unit _, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<Unit, Failure<HandlerFailureCode>>>(cancellationToken);
        }

        return InnerHandleAsync(cancellationToken);
    }

    private async ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandleAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Check if the change log table exists");
        await changeLogApi.EnsureTableAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Get the last change log from the database");
        var dbChangeLogId = await changeLogApi.GetLastChangeLogIdAsync(cancellationToken).ConfigureAwait(false);

        var notExecutedMigrations = await migrationItemApi.GetMigrationItemsAsync(dbChangeLogId?.Id, cancellationToken).ConfigureAwait(false);
        if (notExecutedMigrations.IsEmpty)
        {
            logger.LogInformation("All migrations were already applied");
            return Result.Success<Unit>(default);
        }

        foreach (var migration in notExecutedMigrations)
        {
            logger.LogInformation("Apply the migration {migrationId}", migration.Id);
            await changeLogApi.ExecuteMigrationQueryAsync(migration, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("All migrations have been applied successfully");
        return Result.Success<Unit>(default);
    }
}