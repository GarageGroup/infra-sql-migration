using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

public readonly record struct SqlMigrationOption
{
    public SqlMigrationOption(FlatArray<SqlMigrationItem> migrations, TimeSpan? timeout)
    {
        Migrations = migrations;
        Timeout = timeout;
    }

    public FlatArray<SqlMigrationItem> Migrations { get; }

    public TimeSpan? Timeout { get; }
}