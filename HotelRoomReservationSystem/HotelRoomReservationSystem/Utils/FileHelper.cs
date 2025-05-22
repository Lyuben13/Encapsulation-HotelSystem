using System.Text.Json;

public static class FileHelper<T>
{
    public static List<T> Load(string file)
    {
        if (!File.Exists(file)) return new List<T>();
        var json = File.ReadAllText(file);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new();
    }

    public static void Save(string file, List<T> data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(file, json);
    }
}
