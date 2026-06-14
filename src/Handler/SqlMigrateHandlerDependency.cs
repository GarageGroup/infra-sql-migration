using System;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class SqlMigrateHandlerDependency
{
    public static Dependency<ISqlMigrateHandler> UseSqlMigrateHandler(this Dependency<ISqlApi, SqlMigrationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Fold<ISqlMigrateHandler>(CreateHandler);

        static SqlMigrateHandler CreateHandler(IServiceProvider serviceProvider, ISqlApi sqlApi, SqlMigrationOption option)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(option);

            return SqlMigrateHandler.InternalCreate(
                changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance, option),
                migrationItemApi: new SqlMigrationItemApi(MigrationFileReader.Instance, option),
                loggerFactory: serviceProvider.GetServiceOrThrow<ILoggerFactory>());
        }
    }
}