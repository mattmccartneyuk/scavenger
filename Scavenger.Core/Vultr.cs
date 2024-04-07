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