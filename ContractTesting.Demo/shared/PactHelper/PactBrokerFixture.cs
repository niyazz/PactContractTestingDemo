using System.Net.Http.Headers;
using PactNet;

namespace PactHelper;

public class PactBrokerFixture : IDisposable
{
    private readonly Uri _pactBrokerUri = new ("http://localhost:9292");
    private readonly string _pactUsername = "admin";
    private readonly string _pactPassword = "pass";
    private readonly PactBrokerPublisher _pactBrokerPublisher;

    public string ConsumerVersion { get; set; } 
    public IPact? PactInfo { get; set; }
    
    public PactBrokerFixture()
    {
        var baseAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_pactUsername}:{_pactPassword}"));
        _pactBrokerPublisher = new PactBrokerPublisher(new HttpClient
        {
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue("Basic", baseAuthenticationString)
            },
            BaseAddress = _pactBrokerUri
        });
    }
    
    public void Dispose()
    {
        Task.Run(async () =>
        {
            var versionSuffix = Guid.NewGuid().ToString().Substring(0, 5);
            var pactJson = await File.ReadAllTextAsync($"{PactInfo.Config.PactDir}/{PactInfo.Consumer}-{PactInfo.Provider}.json");
            await _pactBrokerPublisher.Publish(
                consumer: PactInfo.Consumer, provider: PactInfo.Provider, content: pactJson,
                $"{ConsumerVersion}-{versionSuffix}");
        });
    }
}