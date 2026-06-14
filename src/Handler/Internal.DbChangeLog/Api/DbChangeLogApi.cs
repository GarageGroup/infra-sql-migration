namespace GarageGroup.Infra;

internal sealed partial class DbChangeLogApi : IDbChangeLogApi
{
    private const int DbTimeoutInSeconds = int.MaxValue;

    private static readonly DbChangeLogCreateTableQuery DbChangeLogCreateTableQuery
        =
        new()
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

    private static readonly DbChangeLogIdLastGetQuery DbChangeLogIdLastGetQuery
        =
        new()
        {
            TimeoutInSeconds = DbTimeoutInSeconds
        };

    private readonly ISqlApi sqlApi;

    private readonly IMigrationFileReader fileReader;

    private readonly SqlMigrationOption option;

    internal DbChangeLogApi(ISqlApi sqlApi, IMigrationFileReader fileReader, SqlMigrationOption option)
    {
        this.sqlApi = sqlApi;
        this.fileReader = fileReader;
        this.option = option;
    }
}