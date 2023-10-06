using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public async ValueTask<DbChangeLogId?> GetLastChangeLogIdAsync(CancellationToken cancellationToken)
    {
        var sqlRequest = new DbQuery(DbChangeLogIdLastGetQuery)
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

        var dbResult = await sqlApi.QueryEntityOrAbsentAsync<DbChangeLogId>(sqlRequest, cancellationToken).ConfigureAwait(false);
        return dbResult.Fold(AsNullable, GetNull);

        static DbChangeLogId? AsNullable(DbChangeLogId dbChangeLogId)
            =>
            dbChangeLogId;

        static DbChangeLogId? GetNull(Unit _)
            =>
            null;
    }
}