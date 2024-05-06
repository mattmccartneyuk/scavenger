using System.Net.Http.Headers;
using System.Text.Json;
using Renci.SshNet;

namespace Scavenger.Core;

public static class Vultr
{
    public static async Task<RootRegionObject> GetLocations()
    {
        var response = await VultrHttpProvider.Get(new HttpResource { Url = "https://api.vultr.com/v2/regions" });
        return JsonSerializer.Deserialize<RootRegionObject>(response)!;
    }

    public static async Task<string> GetAllInstances(string apiKey)
    {
        var response = await VultrHttpProvider.Get(new HttpResource { Url = "https://api.vultr.com/v2/instances"});
        return response;
    }

    public static async Task<CreateResponse> CreateInstance()
    {
        var response = await VultrHttpProvider.Post(new HttpResource { Url = "https://api.vultr.com/v2/instances", RequestBody = "{\"region\":\"lhr\",\"plan\":\"vc2-1c-1gb\",\"os_id\":\"2179\"}" });
        var instance = JsonSerializer.Deserialize<RootInstanceObject>(response);
        return new CreateResponse
        {
            Instance = instance.Instances,
            Response = response
        };
    }

    public static async Task<string> SendSsh(Instance instance)
    {
        var connectionInfo = new ConnectionInfo(instance.IpAddress, instance.DefaultUser,
            new PasswordAuthenticationMethod(instance.DefaultUser, instance.Password));

        using (var client = new SshClient(connectionInfo))
        {
            client.Connect();

            var command = "mkdir HelloWorld";

            var result = client.RunCommand(command);

            client.Disconnect();

            return result.Result;
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