namespace Scavenger.Core;

public static class Vultr
{
    public static async Task<string> GetLocations()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://api.vultr.com/v2/regions");

                var body = response.Content.ReadAsStringAsync();

                return body.Result;
            }
        }
        catch (Exception e)
        {
            return "nothing";
        }
    }
}