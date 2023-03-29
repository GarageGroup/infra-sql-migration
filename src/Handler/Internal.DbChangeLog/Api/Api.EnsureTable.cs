using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DbChangeLogApi
{
    public async ValueTask EnsureTableAsync(CancellationToken cancellationToken)
        =>
        await sqlApi.ExecuteNonQueryAsync(new DbQuery(DbChangeLogCreateTableQuery), cancellationToken).ConfigureAwait(false);
}