namespace Phos.MusicManager.Library.Common.Serializers;

using System.Text.Json;

#pragma warning disable SA1600 // Elements should be documented
public static class JsonFileSerializer
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static void Serialize(string file, object? value)
    {
        File.WriteAllText(file, JsonSerializer.Serialize(value, Options));
    }

    public static T? Deserialize<T>(string file)
    {
        var value = JsonSerializer.Deserialize<T>(File.ReadAllText(file));
        return value;
    }
}
