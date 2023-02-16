using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal interface ISqlMigrationItemApi
{
    ValueTask<FlatArray<SqlMigrationItem>> GetMigrationItemsAsync(string? migrationItemId, CancellationToken cancellationToken);
}