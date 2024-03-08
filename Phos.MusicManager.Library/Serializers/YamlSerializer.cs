namespace Phos.MusicManager.Library.Serializers;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

#pragma warning disable SA1600 // Elements should be documented
public static class YamlSerializer
{
    private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .DisableAliases()
            .Build();

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .Build();

    public static void Serialize(string file, object? value)
    {
        var dir = Path.GetDirectoryName(file);
        if (dir != null)
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(file, Serializer.Serialize(value));
    }

    public static T? Deserialize<T>(string file)
    {
        var value = Deserializer.Deserialize<T>(File.ReadAllText(file));
        return value;
    }
}