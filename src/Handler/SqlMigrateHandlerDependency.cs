using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class SqlMigrateHandlerDependency
{
    private const string DefaultConfigPathKey = "Migrations:ConfigPath";

    public static Dependency<SqlMigrateHandler> UseSqlMigrateHandler(
        this Dependency<ISqlApi> dependency, string configPathKey = DefaultConfigPathKey)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map(CreateHandler);

        SqlMigrateHandler CreateHandler(IServiceProvider serviceProvider, ISqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(sqlApi);

            var option = serviceProvider.GetServiceOrThrow<IConfiguration>().GetSqlMigrationOption(configPathKey ?? string.Empty);

            return SqlMigrateHandler.InternalCreate(
                changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance),
                migrationItemApi: new SqlMigrationItemApi(MigrationFileReader.Instance, option),
                loggerFactory: serviceProvider.ResolveLoggerFactory());
        }
    }

    public static Dependency<SqlMigrateHandler> UseSqlMigrateHandler(
        this Dependency<ISqlApi> dependency, Func<IServiceProvider, SqlMigrationOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        ArgumentNullException.ThrowIfNull(optionResolver);

        return dependency.With(optionResolver).Fold(CreateHandler);

        static SqlMigrateHandler CreateHandler(IServiceProvider serviceProvider, ISqlApi sqlApi, SqlMigrationOption option)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(sqlApi);

            return SqlMigrateHandler.InternalCreate(
                changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance),
                migrationItemApi: new SqlMigrationItemApi(MigrationFileReader.Instance, option),
                loggerFactory: serviceProvider.ResolveLoggerFactory());
        }
    }

    public static Dependency<SqlMigrateHandler> UseSqlMigrateHandler(
        this Dependency<ISqlApi, ILoggerFactory, SqlMigrationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateHandler);

        static SqlMigrateHandler CreateHandler(ISqlApi sqlApi, ILoggerFactory loggerFactory, SqlMigrationOption option)
        {
            ArgumentNullException.ThrowIfNull(sqlApi);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            return SqlMigrateHandler.InternalCreate(
                changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance),
                migrationItemApi: new SqlMigrationItemApi(MigrationFileReader.Instance, option),
                loggerFactory: loggerFactory);
        }
    }

    private static ILoggerFactory ResolveLoggerFactory(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetServiceOrThrow<ILoggerFactory>();

    private static SqlMigrationOption GetSqlMigrationOption(this IConfiguration configuration, string configPathKey)
        =>
        new(
            configPath: configuration[configPathKey]);
}