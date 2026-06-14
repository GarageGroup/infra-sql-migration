using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public Task EnsureTableAsync(CancellationToken cancellationToken)
        =>
        sqlApi.ExecuteNonQueryAsync(DbChangeLogCreateTableQuery, cancellationToken).AsTask();
}