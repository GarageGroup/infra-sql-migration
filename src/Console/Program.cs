using System.Threading.Tasks;

namespace GarageGroup.Infra.Sql.Migration.Console;

static class Program
{
    static Task Main(string[] args)
        =>
        Application.UseSqlMigrateHandler().UseConsoleRunner(args).RunAsync();
}