﻿using System.Net.Http.Json;
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

        // в реальном приложении здесь мы пишем какие-то логи или делаем что-то еще
        return null;
    }
}