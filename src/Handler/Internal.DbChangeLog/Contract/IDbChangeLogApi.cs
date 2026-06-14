using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal interface IDbChangeLogApi
{
    Task EnsureTableAsync(CancellationToken cancellationToken);

    Task<DbChangeLogId?> GetLastChangeLogIdAsync(CancellationToken cancellationToken);

    Task ExecuteMigrationQueryAsync(SqlMigrationItem migrationItem, CancellationToken cancellationToken);
}