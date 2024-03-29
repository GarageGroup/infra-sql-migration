using System;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

public sealed partial class SqlMigrateHandler : IHandler<Unit, Unit>
{
    internal static SqlMigrateHandler InternalCreate(
        IDbChangeLogApi changeLogApi, ISqlMigrationItemApi migrationItemApi, ILoggerFactory loggerFactory)
        =>
        new(
            changeLogApi, migrationItemApi, loggerFactory.CreateLogger<SqlMigrateHandler>());

    private readonly IDbChangeLogApi changeLogApi;

    private readonly ISqlMigrationItemApi migrationItemApi;

    private readonly ILogger logger;

    private SqlMigrateHandler(IDbChangeLogApi changeLogApi, ISqlMigrationItemApi migrationItemApi, ILogger logger)
    {
        this.changeLogApi = changeLogApi;
        this.migrationItemApi = migrationItemApi;
        this.logger = logger;
    }
}