using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GarageGroup.Infra;

public static class SqlMigrateHandlerDependency
{
    private const string DefaultConfigSection = "Migrations";

    public static Dependency<SqlMigrateHandler> UseSqlMigrateHandler(
        this Dependency<ISqlApi> dependency, string configSectionName = DefaultConfigSection)
    {
        ArgumentNullException.ThrowIfNull(dependency);

        return dependency.Map(CreateHandler);

        SqlMigrateHandler CreateHandler(IServiceProvider serviceProvider, ISqlApi sqlApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(sqlApi);

            var option = serviceProvider.GetServiceOrThrow<IConfiguration>().GetRequiredSection(configSectionName).GetSqlMigrationOption();

            return SqlMigrateHandler.InternalCreate(
                changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance, option),
                migrationItemApi: new SqlMigrationItemApi(MigrationFileReader.Instance, option),
                loggerFactory: serviceProvider.ResolveLoggerFactory());
        }
    }

    private static ILoggerFactory ResolveLoggerFactory(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetServiceOrThrow<ILoggerFactory>();

    private static SqlMigrationOption GetSqlMigrationOption(this IConfigurationSection section)
        =>
        new(
            configPath: section["ConfigPath"])
        {
            BasePath = section["BasePath"]
        };
}