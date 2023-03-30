namespace GGroupp.Infra;

internal readonly record struct ConfigurationYaml
{
    public MigrationItemYaml[]? Migrations { get; init; }
}