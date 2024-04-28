using System.Text.Json.Serialization;

namespace Scavenger.Core;

public class RootInstanceObject
{
    [JsonPropertyName("instance")]
    public Instance? Instances { get; set; }
}

public class Instance
{
    [JsonPropertyName("user_scheme")]
    public string? DefaultUser { get; set; }

    [JsonPropertyName("default_password")]
    public string? Password { get; set; }

    [JsonPropertyName("main_ip")]
    public string? IpAddress { get; set; }

    public void Update(string ipAddress, string defaultUser, string password)
    {
        DefaultUser = defaultUser;
        Password = password;
        IpAddress = ipAddress;
    }
}

public struct CreateResponse
{
    public Instance Instance { get; set; }
    public string Response { get; set; }
}