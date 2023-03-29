using System.Threading.Tasks;

namespace GGroupp.Infra.Sql.Migration.Console;

static class Program
{
    static Task Main(string[] args)
        =>
        MicrosoftDbProvider.Configure("SqlDb")
        .UseSqlApi()
        .UseSqlMigrateHandler("Migrations")
        .RunConsoleAsync(args);
}