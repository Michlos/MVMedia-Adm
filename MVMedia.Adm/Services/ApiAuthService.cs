using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MVMedia.Adm.Services;

public class ApiAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiAuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<UserToken?> LoginAsync(string username, string password)
    {
        var client = _httpClientFactory.CreateClient("MVMediaAPI");
        var loginData = new { Username = username, Password = password };
        var response = await client.PostAsJsonAsync("api/User/login", loginData);

        if (!response.IsSuccessStatusCode)
            return null;

        try
        {

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await response.Content.ReadFromJsonAsync<UserToken>(options);

        }
        catch (JsonException)
        {

            return null;
        }

        
    }


}

public class UserToken
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
    [JsonPropertyName("isAdmin")]
    public bool IsAdmin { get; set; }
}
