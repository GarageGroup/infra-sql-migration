using System;

namespace GGroupp.Infra;

[DbEntity]
internal sealed partial record class DbChangeLogId : IDbEntity<DbChangeLogId>
{
    private readonly string? id;

    [DbField]
    public string Id
    {
        get => id.OrEmpty();
        init => id = value.OrNullIfEmpty();
    }
}