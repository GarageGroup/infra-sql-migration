using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed record class SqlMigrationItem
{
    public SqlMigrationItem(string id, string filePath, [AllowNull] string comment = null)
    {
        Id = id ?? string.Empty;
        FilePath = filePath ?? string.Empty;
        Comment = string.IsNullOrEmpty(comment) ? null : comment;
    }

    public string Id { get; }

    public string FilePath { get; }

    public string? Comment { get; }
}