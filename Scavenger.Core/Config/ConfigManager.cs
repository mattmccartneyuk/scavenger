using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scavenger.Core.Config;

public class ConfigManager
{
    public static string GetApiKey()
    {
        var path = Environment.ProcessPath;

        var jsonText = File.ReadAllText(Path.Combine(path, "..", "Config", "config.json"));

        return JsonSerializer.Deserialize<ConfigJsonMap>(jsonText)!.ApiKey;
    }

    public static void SetApiKey(string newApiKey)
    {
        var path = Environment.ProcessPath;

        var json = new ConfigJsonMap
        {
            ApiKey = newApiKey 
        };

        var newJson = JsonSerializer.Serialize(json);

        File.WriteAllText(Path.Combine(path, "..", "Config", "config.json"), newJson);
    }
}

public class ConfigJsonMap
{
    [JsonPropertyName("APIKey")]
    public required string ApiKey { get; set; }
}