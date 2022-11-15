using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GGroupp.Infra;

public static class SqlMigrationApplication
{
    public static async Task RunMigrationAsync(this Dependency<ISqlApi> dependency, [AllowNull] string[] args = null)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        using var serviceProvider = CreateServiceProvider(args ?? Array.Empty<string>());
        await dependency.Map(ResolveMigration).Resolve(serviceProvider).RunAsync().ConfigureAwait(false);
    }

    private static SqlMigration ResolveMigration(IServiceProvider serviceProvider, ISqlApi sqlApi)
    {
        _ = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _ = sqlApi ?? throw new ArgumentNullException(nameof(sqlApi));

        return SqlMigration.InternalCreate(
            changeLogApi: new DbChangeLogApi(sqlApi, MigrationFileReader.Instance),
            loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>(),
            option: serviceProvider.GetRequiredService<IConfiguration>().GetSqlMigrationOption());
    }

    private static SqlMigrationOption GetSqlMigrationOption(this IConfiguration configuration)
        =>
        new(
            migrations: configuration.GetSection("Migrations").Get<SqlMigrationItem[]>(),
            timeout: configuration.GetValue<TimeSpan?>("Timeout"));

    private static ServiceProvider CreateServiceProvider(string[] args)
        =>
        new ServiceCollection()
        .AddLogging(
            static builder => builder.AddConsole())
        .AddSingleton(
            BuildConfiguration(args))
        .BuildServiceProvider();

    private static IConfiguration BuildConfiguration(string[] args)
        =>
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();
}