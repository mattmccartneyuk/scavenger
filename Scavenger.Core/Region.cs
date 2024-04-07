using System.Text.Json.Serialization;

namespace Scavenger.Core;

public class RootRegionObject
{
    [JsonPropertyName("regions")]
    public required List<Region>? Regions { get; set; }
}
public class Region
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("city")]
    public required string City { get; set; }

    [JsonPropertyName("country")]
    public required string Country { get; set; }

    [JsonPropertyName("continent")]
    public required string Continent { get; set; }

}