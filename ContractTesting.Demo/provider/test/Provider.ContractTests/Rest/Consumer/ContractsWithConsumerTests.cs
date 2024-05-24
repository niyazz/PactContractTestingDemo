using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PactNet.Verifier;
using Provider.Contracts.Models;
using Provider.Domain;
using Provider.Domain.Models;
using Provider.Host;
using Xunit;
using Xunit.Abstractions;

namespace Provider.ContractTests.Rest.Consumer;


public class ContractsWithConsumerTests : IDisposable
{
    private readonly Uri _serverUri = new ("http://localhost:5000");
    private const string ComType = "REST";
    private readonly Mock<ICardAccountsRepository> _cardAccountsRepository;
    private readonly PactVerifier _pactVerifier;
    private bool _disposedValue;
    
    private readonly IHostBuilder _serverBuilder;
    private IHost _server;
    private readonly string _providerVersion;

    public ContractsWithConsumerTests(ITestOutputHelper outputHelper)
    {
        _cardAccountsRepository = new Mock<ICardAccountsRepository>();
        var config = new PactVerifierConfig
        {
            Outputters = new []{ new PactXUnitOutput(outputHelper) }
        };
        _pactVerifier = new PactVerifier("Demo.Provider", config);
        Environment.SetEnvironmentVariable("PACT_DO_NOT_TRACK", "true");
        _serverBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(_serverUri.ToString());
                webBuilder.UseStartup<Startup>();
            }); 
        _providerVersion = Assembly.GetAssembly(typeof(UserCardAccountsResponse))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion!;
    }
    
    [Fact(DisplayName = "Rest контракты с потребителем Demo.Consumer соблюдаются")]
    public void Verify_RestDemoConsumerContacts()
    {
        // Arrange
        var successId = "successId1";
        var failureId = "failureId1";
        var accountId = "accountId";
        _cardAccountsRepository.Setup(x => x.GetCardAccountsByUserId(successId))
            .ReturnsAsync(DataForTests.UserCardAccountsSuccessResult);
        _cardAccountsRepository.Setup(x => x.AddCard(successId, accountId, It.IsAny<bool>()))
            .ReturnsAsync(DataForTests.CardSuccessResult);
        _cardAccountsRepository.Setup(x => x.GetCardAccountsByUserId(failureId))
            .ReturnsAsync((UserCardAccounts?)null);
        _cardAccountsRepository.Setup(x => x.AddCard(failureId, accountId, It.IsAny<bool>()))
            .ReturnsAsync((CardInfo?)null);

        _serverBuilder.ConfigureServices(services => 
                services.AddSingleton<ICardAccountsRepository>(_ => _cardAccountsRepository.Object));
        _server = _serverBuilder.Build();
        _server.StartAsync();
        
        // Act & Assert
        _pactVerifier
            .WithHttpEndpoint(_serverUri)
            .WithPactBrokerSource(new Uri("http://localhost:9292"), options =>
            {
                options.BasicAuthentication("admin", "pass");
                options.PublishResults(_providerVersion);
            })
            // .WithFileSource(new FileInfo(@"..\..\..\pacts\Demo.Consumer-Demo.Provider.json"))
            .WithFilter(ComType)
            .Verify();
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _server.StopAsync().GetAwaiter().GetResult();
                _server.Dispose();
                _pactVerifier.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose() => Dispose(true);
}