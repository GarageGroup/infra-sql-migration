namespace GGroupp.Infra;

internal sealed partial class DbChangeLogApi : IDbChangeLogApi
{
    private const string DbChangeLogCreateTableQuery = """
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='_ChangeLogHistory' and xtype='U')
            BEGIN
            CREATE TABLE [_ChangeLogHistory]( 
                [Id] varchar(100) NOT NULL,
                [Comment] nvarchar (255) NULL,
                [CreationTime] datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
                PRIMARY KEY (Id)
            );
            CREATE INDEX ChangeLogHistoryCreationTimeIndex ON [_ChangeLogHistory](CreationTime DESC);
            END
        """;

    private const string DbChangeLogIdLastGetQuery = "SELECT TOP 1 Id From [_ChangeLogHistory] ORDER BY CreationTime DESC;";

    private const string DbChangeLogInsertQuery = "INSERT INTO [_ChangeLogHistory](Id, Comment) VALUES (@Id, @Comment);";

    private readonly ISqlApi sqlApi;

    private readonly IMigrationFileReader fileReader;

    internal DbChangeLogApi(ISqlApi sqlApi, IMigrationFileReader fileReader)
    {
        this.sqlApi = sqlApi;
        this.fileReader = fileReader;
    }

    
}