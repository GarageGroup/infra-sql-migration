using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal sealed class MigrationFileReader : IMigrationFileReader
{
    static MigrationFileReader()
        =>
        Instance = new();

    public static MigrationFileReader Instance { get; }

    private MigrationFileReader()
    {
    }

    public Task<string> ReadMigrationQueryAsync(string filePath, CancellationToken cancellationToken)
        =>
        File.ReadAllTextAsync(filePath, cancellationToken);
}