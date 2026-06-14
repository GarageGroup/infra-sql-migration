using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

public sealed record class SqlMigrationOption
{
    private const string DefaultConfigPath = "migration.yaml";

    public SqlMigrationOption([AllowNull] string configPath = DefaultConfigPath)
        =>
        ConfigPath = string.IsNullOrWhiteSpace(configPath) ? DefaultConfigPath : configPath;

    public string ConfigPath { get; }

    public string? BasePath { get; init; }
}