namespace GGroupp.Infra;

internal sealed record class MigrationItemYaml
{
    public string? Id { get; init; }

    public string? FilePath { get; init; }

    public string? Comment { get; init; }
}