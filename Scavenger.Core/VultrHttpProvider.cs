using System.Net.Http.Headers;

namespace Scavenger.Core;

public static class VultrHttpProvider 
{
    public static async Task<string> Get(HttpResource resource)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resource.ApiKey);
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
}

public struct HttpResource
{
    public string ApiKey { get; set; }
    public string Url { get; set; }
}