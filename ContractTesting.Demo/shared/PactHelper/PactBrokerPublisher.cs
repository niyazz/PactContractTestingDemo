using System.Net.Http.Headers;

namespace PactHelper;

internal class PactBrokerPublisher
{
    private readonly HttpClient _httpClient;
    
    public PactBrokerPublisher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task Publish(string consumer, string provider, string pactDir, string consumerVersion)
    {
        var pactJson = await File.ReadAllTextAsync($"{pactDir}/{consumer}-{provider}.json");
        var response = await _httpClient.PutAsync($"pacts/provider/{provider}/consumer/{consumer}/version/{consumerVersion}",
            new StringContent(pactJson)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });

        if (response.IsSuccessStatusCode == false)
            throw new ArgumentNullException($"{response.StatusCode}");
    } 
}