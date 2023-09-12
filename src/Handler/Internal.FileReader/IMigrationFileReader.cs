using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal interface IMigrationFileReader
{
    Task<string> ReadAsync(string? basePath, string filePath, CancellationToken cancellationToken);
}