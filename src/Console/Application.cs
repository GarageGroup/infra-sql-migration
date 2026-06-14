using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Infra;

internal static class Application
{
    public static Dependency<ISqlMigrateHandler> UseSqlMigrateHandler()
        =>
        MicrosoftDbProvider.Configure("SqlDb")
        .UseSqlApi()
        .With(ResolveSqlMigrationOption)
        .UseSqlMigrateHandler();

    private static SqlMigrationOption ResolveSqlMigrationOption(IServiceProvider serviceProvider)
    {
        var section = serviceProvider.GetRequiredService<IConfiguration>().GetRequiredSection("Migrations");

        return new(
            configPath: section["ConfigPath"])
        {
            BasePath = section["BasePath"]
        };
    }
}