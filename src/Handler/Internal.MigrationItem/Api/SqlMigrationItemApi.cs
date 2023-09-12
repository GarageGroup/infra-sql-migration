using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GarageGroup.Infra;

internal sealed partial class SqlMigrationItemApi : ISqlMigrationItemApi
{
    private static readonly IDeserializer YamlDeserializer;

    static SqlMigrationItemApi()
        =>
        YamlDeserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

    private readonly IMigrationFileReader fileReader;

    private readonly SqlMigrationOption option;

    internal SqlMigrationItemApi(IMigrationFileReader fileReader, SqlMigrationOption option)
    {
        this.fileReader = fileReader;
        this.option = option;
    }
}