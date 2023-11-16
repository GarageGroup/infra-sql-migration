using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed record class SqlMigrationItem
{
    public SqlMigrationItem(string id, string filePath, [AllowNull] string comment = null)
    {
        Id = id.OrEmpty();
        FilePath = filePath.OrEmpty();
        Comment = comment.OrNullIfEmpty();
    }

    public string Id { get; }

    public string FilePath { get; }

    public string? Comment { get; }
}