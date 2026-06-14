using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DbChangeLogApi
{
    public async Task<DbChangeLogId?> GetLastChangeLogIdAsync(CancellationToken cancellationToken)
    {
        var dbResult = await sqlApi.QueryEntityOrAbsentAsync<DbChangeLogId>(DbChangeLogIdLastGetQuery, cancellationToken).ConfigureAwait(false);
        return dbResult.Fold(AsNullable, GetNull);

        static DbChangeLogId? AsNullable(DbChangeLogId dbChangeLogId)
            =>
            dbChangeLogId;

        static DbChangeLogId? GetNull(Unit _)
            =>
            null;
    }
}