using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public async ValueTask EnsureTableAsync(CancellationToken cancellationToken)
    {
        var query = new DbQuery(DbChangeLogCreateTableQuery)
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        _ = await sqlApi.ExecuteNonQueryAsync(query, cancellationToken).ConfigureAwait(false);
    }
}