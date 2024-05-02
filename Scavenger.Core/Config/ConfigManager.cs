﻿using System.Text.Json;
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
}

public class ConfigJsonMap
{
    [JsonPropertyName("APIKey")]
    public required string ApiKey { get; set; }
}