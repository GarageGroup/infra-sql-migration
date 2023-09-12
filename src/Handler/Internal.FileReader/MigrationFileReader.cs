using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed class MigrationFileReader : IMigrationFileReader
{
    static MigrationFileReader()
        =>
        Instance = new();

    public static MigrationFileReader Instance { get; }

    private MigrationFileReader()
    {
    }

    public Task<string> ReadAsync(string? basePath, string filePath, CancellationToken cancellationToken)
    {
        var fullPath = string.IsNullOrEmpty(basePath) switch
        {
            false => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, basePath, filePath),
            _ => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath)
        };

        return File.ReadAllTextAsync(fullPath, cancellationToken);
    }
}