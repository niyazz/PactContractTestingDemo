using System.Net.Http.Headers;
using PactNet;

namespace PactHelper;

public class PactBrokerSender : IDisposable
{
    protected readonly Uri PactBrokerUri = new Uri("http://localhost:9292");
    protected readonly string PactUsername = "admin";
    protected readonly string PactPassword = "pass";
    private readonly PactBrokerPublisher _pactBrokerPublisher;

    public string? ConsumerVersion { get; set; } 
    public IPact? PactInfo { get; set; }
    
    public PactBrokerSender()
    {
        var baseAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{PactUsername}:{PactPassword}"));
        _pactBrokerPublisher = new PactBrokerPublisher(new HttpClient
        {
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue("Basic", baseAuthenticationString)
            },
            BaseAddress = PactBrokerUri
        });
    }
    
    public void Dispose()
    {
        Task.Run(async () => await _pactBrokerPublisher.Publish(
            consumer: PactInfo.Consumer, provider: PactInfo.Provider, PactInfo.Config.PactDir, "5"));
    }
}