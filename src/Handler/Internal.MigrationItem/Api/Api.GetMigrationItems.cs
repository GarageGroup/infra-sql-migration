using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlMigrationItemApi
{
    public async ValueTask<FlatArray<SqlMigrationItem>> GetMigrationItemsAsync(
        string? migrationItemId, CancellationToken cancellationToken)
    {
        var yaml = await fileReader.ReadAsync(option.BasePath, option.ConfigPath, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(yaml))
        {
            return default;
        }

        var migrations = YamlDeserializer.Deserialize<ConfigurationYaml>(yaml).Migrations?.Select(MapItem).ToArray();
        if (migrations?.Length is not > 0)
        {
            return default;
        }

        if (string.IsNullOrEmpty(migrationItemId))
        {
            return new(migrations.ToFlatArray());
        }

        var lastMigration = migrations.FirstOrDefault(IsMigrationIdMatched);
        if (lastMigration is null)
        {
            return new(migrations.ToFlatArray());
        }

        var resultList = new List<SqlMigrationItem>(migrations.Length);
        var alreadyFound = false;

        foreach (var migrationItem in migrations)
        {
            if (alreadyFound)
            {
                resultList.Add(migrationItem);
                continue;
            }

            if (migrationItem == lastMigration)
            {
                alreadyFound = true;
            }
        }

        return resultList;

        bool IsMigrationIdMatched(SqlMigrationItem migrationItem)
            =>
            string.Equals(migrationItemId, migrationItem.Id, StringComparison.InvariantCulture);
    }

    private static SqlMigrationItem MapItem(MigrationItemYaml itemYaml)
        =>
        new(
            id: itemYaml.Id ?? string.Empty,
            filePath: itemYaml.FilePath ?? string.Empty,
            comment: itemYaml.Comment);
}