using System.Net.Http.Headers;
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

    public static async Task<string> CreateInstance()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.vultr.com/v2/instances";

                string requestBody = "{\"region\":\"lhr\",\"plan\":\"vc2-1c-1gb\",\"os_id\":\"2179\"}";

                HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

                HttpResponseMessage response = await client.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception e)
        {
            return "Failed HttpClient Connection.";
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
            if (!Lookup.ContainsKey(region.Continent))
            {
                var country = new Dictionary<string, HashSet<string>>();
                country.Add(region.Country, new HashSet<string>
                {
                    region.City
                });

                Lookup.Add(region.Continent, country);
            }
            else
            {
                if (!Lookup[region.Continent].ContainsKey(region.Country))
                {
                    var city = new HashSet<string>
                    {
                        region.City
                    };

                    Lookup[region.Continent].Add(region.Country, city);
                }
                else
                {
                    Lookup[region.Continent][region.Country].Add(region.City);
                }
            }
        }
    }
}