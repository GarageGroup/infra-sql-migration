using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public readonly record struct SqlMigrationOption
{
    private const string DefaultConfigPath = "migration.yaml";

    private readonly string? configPath;

    public SqlMigrationOption([AllowNull] string configPath = DefaultConfigPath)
        =>
        this.configPath = string.IsNullOrEmpty(configPath) ? null : configPath;

    public string ConfigPath
        =>
        string.IsNullOrEmpty(configPath) ? DefaultConfigPath : configPath;

    public string? BasePath { get; init; }
}