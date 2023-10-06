using System;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Sql.Migration.Console;

static class Program
{
    static Task Main(string[] args)
        =>
        MicrosoftDbProvider.Configure("SqlDb")
        .UseSqlApi()
        .UseSqlMigrateHandler("Migrations")
        .RunConsoleAsync<SqlMigrateHandler, Unit>(default, args);
}