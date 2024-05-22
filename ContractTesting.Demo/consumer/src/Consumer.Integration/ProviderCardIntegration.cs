using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Consumer.Integration.ProviderContracts.V1;

namespace Consumer.Integration;

public class ProviderCardIntegration 
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public ProviderCardIntegration(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserCardAccountsDto?> GetCardAccountInfo(string userId)
    {
        var response = await _httpClient.GetAsync($"api/provider/cards/accounts/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserCardAccountsDto>(JsonSerializerOptions);
        }
        
        return null;
    }
    
    public async Task<CardDto?> OrderCard(string userId, string accountId, bool isNamed)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            isNamed
        }), Encoding.UTF8, "application/json");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await _httpClient.PostAsync($"api/provider/cards/{userId}?accountId={accountId}", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CardDto>(JsonSerializerOptions);
        }
        
        return null;
    }
}