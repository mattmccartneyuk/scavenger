using System.Net.Http.Headers;
using Scavenger.Core.Config;

namespace Scavenger.Core;

public static class VultrHttpProvider 
{
    public static async Task<string> Get(HttpResource resource)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigManager.GetApiKey());
                HttpResponseMessage response = await client.GetAsync(resource.Url);

                string body = await response.Content.ReadAsStringAsync();

                return body;
            }
        }
        catch (Exception e)
        {
            return "Not Found";
        }
    }

    public static async Task<string> Post(HttpResource resource)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(resource.RequestBody, System.Text.Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigManager.GetApiKey());

                HttpResponseMessage response = await client.PostAsync(resource.Url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception e)
        {
            return "Failed HttpClient Connection.";

        }
    }
}

public struct HttpResource
{
    public string Url { get; set; }
    public string RequestBody { get; set; }
}