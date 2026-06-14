using System;

namespace GarageGroup.Infra;

internal sealed record class DbChangeLogCreateTableQuery : IDbQuery
{
    private const string TransactSqlQuery
        =
        """
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

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery(SqlDialect dialect)
    {
        if (dialect is SqlDialect.TransactSql)
        {
            return TransactSqlQuery;
        }

        throw new NotSupportedException($"Dialect '{dialect}' is not supported for {nameof(DbChangeLogCreateTableQuery)}.");
    }

    public FlatArray<DbParameter> GetParameters()
        =>
        default;
}