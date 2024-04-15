using System.Text.Json;

namespace Scavenger.Core;

public static class Vultr
{
    public static async Task<RootRegionObject> GetLocations()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://api.vultr.com/v2/regions");

                string body = await response.Content.ReadAsStringAsync();

                RootRegionObject? regions = JsonSerializer.Deserialize<RootRegionObject>(body);

                return regions;
            }
        }
        catch (Exception e)
        {
            return new RootRegionObject
            {
                Regions = null
            };
        }
    }
}

public class ContinentToCountryToCity
{
    public Dictionary<string, Dictionary<string, HashSet<string>>>? Lookup { get; set; } = new();

    public void Parse(RootRegionObject regions)
    {
        foreach (var region in regions.Regions)
        {
            var countryToCityDictionary = new Dictionary<string, HashSet<string>>();

            if (!countryToCityDictionary.ContainsKey(region.Country))
            {
                countryToCityDictionary.Add(region.Country, new HashSet<string>{region.City});
            }
            else
            {
                countryToCityDictionary[region.City].Add(region.City);
            }

            if (!Lookup.ContainsKey(region.Continent))
            {
                Lookup.Add(region.Continent, countryToCityDictionary);
            }
            else
            {
                Lookup[region.Continent][region.Country] = new HashSet<string> { region.City };
            }
        }
    }
}