using System.Net.Http.Headers;
using System.Text.Json;

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
        var response = await VultrHttpProvider.Get(new HttpResource { Url = "https://api.vultr.com/v2/instances", ApiKey = apiKey });
        return response;
    }

    public static async Task<CreateResponse> CreateInstance(string apiKey)
    {
        //var response = await VultrHttpProvider.Post(new HttpResource { Url = "https://api.vultr.com/v2/instances", ApiKey = apiKey, RequestBody = "{\"region\":\"lhr\",\"plan\":\"vc2-1c-1gb\",\"os_id\":\"2179\"}" });
        var response =
            @"{""instance"":{""id"":""4f0f12e5-1f84-404f-aa84-85f431ea5ec2"",""os"":""CentOS 8 Stream"",""ram"":1024,""disk"":0,""main_ip"":""0.0.0.0"",""vcpu_count"":1,""region"":""ewr"",""plan"":""vc2-1c-1gb"",""date_created"":""2021-09-14T13:22:20+00:00"",""status"":""pending"",""allowed_bandwidth"":1000,""netmask_v4"":"""",""gateway_v4"":""0.0.0.0"",""power_status"":""running"",""server_status"":""none"",""v6_network"":"""",""v6_main_ip"":"""",""v6_network_size"":0,""label"":"""",""internal_ip"":"""",""kvm"":"""",""hostname"":""my_hostname"",""os_id"":401,""app_id"":0,""image_id"":"""",""firewall_group_id"":"""",""features"":[],""default_password"":""v5{Fkvb#2ycPGwHs"",""tags"":[""a tag"",""another""],""user_scheme"":""root""}}";
        var instance = JsonSerializer.Deserialize<RootInstanceObject>(response);
        return new CreateResponse
        {
            Instance = instance.Instances,
            Response = response
        };
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