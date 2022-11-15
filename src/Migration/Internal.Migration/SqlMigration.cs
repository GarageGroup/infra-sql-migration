using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed class SqlMigration
{
    internal static SqlMigration InternalCreate(IDbChangeLogApi changeLogApi, ILoggerFactory loggerFactory, SqlMigrationOption option)
        =>
        new(
            changeLogApi, loggerFactory.CreateLogger<SqlMigration>(), option);

    private readonly IDbChangeLogApi changeLogApi;

    private readonly ILogger logger;

    private readonly SqlMigrationOption option;

    private SqlMigration(IDbChangeLogApi changeLogApi, ILogger logger, SqlMigrationOption option)
    {
        this.changeLogApi = changeLogApi;
        this.logger = logger;
        this.option = option;
    }

    public ValueTask RunAsync()
    {
        if (option.Migrations.IsEmpty)
        {
            logger.LogInformation("Migrations list is absent. The operation has finished");
            return ValueTask.CompletedTask;
        }

        return InnerRunAsync();
    }

    private async ValueTask InnerRunAsync()
    {
        try
        {
            using var cancellationTokenSource = GetCancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            logger.LogInformation("Check if ChangeLog table exists");
            await changeLogApi.EnsureTableAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Get the last change log from the database");
            var dbChangeLogId = await changeLogApi.GetLastChangeLogIdAsync(cancellationToken).ConfigureAwait(false);

            var notExecutedMigrations = GetNotExecutedMigrations(dbChangeLogId);
            if (notExecutedMigrations.Count is not > 0)
            {
                logger.LogInformation("All migrations were already executed");
                return;
            }

            foreach (var migration in notExecutedMigrations)
            {
                logger.LogInformation("Execute migration {migrationId}", migration.Id);
                await changeLogApi.ExecuteMigrationQueryAsync(migration, cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("All migration have been finished successfully");
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "The operation was canceled");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected exception was thrown");
            throw;
        }
    }

    private IReadOnlyCollection<SqlMigrationItem> GetNotExecutedMigrations(DbChangeLogId? dbChangeLogId)
    {
        if (dbChangeLogId is null)
        {
            return option.Migrations;
        }

        var lastMigration = option.Migrations.FirstOrDefault(IsLastMigration);
        if (lastMigration is null)
        {
            return option.Migrations;
        }

        var resultList = new List<SqlMigrationItem>(option.Migrations.Length);
        var alreadyFound = false;

        foreach (var migrationItem in option.Migrations)
        {
            if (alreadyFound)
            {
                resultList.Add(migrationItem);
                continue;
            }

            if (migrationItem == lastMigration)
            {
                alreadyFound = true;
                continue;
            }
        }

        return resultList;

        bool IsLastMigration(SqlMigrationItem migrationItem)
            =>
            string.Equals(dbChangeLogId.Id, migrationItem.Id, StringComparison.InvariantCulture);
    }

    private CancellationTokenSource GetCancellationTokenSource()
        =>
        option.Timeout is null ? new() : new(option.Timeout.Value);
}