using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed partial record class DbChangeLogId : IDbEntity<DbChangeLogId>
{
    public static DbChangeLogId ReadEntity(IDbItem dbItem)
    {
        ArgumentNullException.ThrowIfNull(dbItem);

        return new()
        {
            Id = dbItem.GetFieldValueOrThrow("Id")
        };
    }

    private readonly string? id;

    [AllowNull]
    public string Id
    {
        get => id.OrEmpty();
        init => id = value.OrNullIfEmpty();
    }
}