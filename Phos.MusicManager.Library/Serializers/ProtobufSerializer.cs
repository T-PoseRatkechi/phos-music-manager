namespace Phos.MusicManager.Library.Serializers;

using ProtoBuf;

#pragma warning disable SA1600 // Elements should be documented
public static class ProtobufSerializer
{
    public static void Serialize(string file, object? value)
    {
        var dir = Path.GetDirectoryName(file);
        if (dir != null)
        {
            Directory.CreateDirectory(dir);
        }

        using var fs = File.Create(file);
        Serializer.Serialize(fs, value);
    }

    public static T Deserialize<T>(string file)
    {
        using var fs = File.OpenRead(file);
        var value = Serializer.Deserialize<T>(fs) ?? throw new InvalidDataException($"Failed to deserialize file.\nFile: {file}");
        return value;
    }
}
