using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Consumer.Integration.ProviderContracts.V1;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;

namespace Consumer.ContractTests.RabbitMq;

public class ContractWithConsumerTests : IDisposable
{
    private readonly PactVerifier _pactVerifier;
    private const string ComType = "RABBITMQ";

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ContractWithConsumerTests(ITestOutputHelper testOutputHelper) 
    {
        _pactVerifier = new PactVerifier("Demo.Provider", new PactVerifierConfig
        {
            Outputters = new []{ new PactXUnitOutput(testOutputHelper) }
        });
    }
    
    [Fact(DisplayName = "Событие оформления заявления на заказ карты из Demo.Provider соответствует контракту")]
    public void Verify_CardOrderApplicationEvent()
    {
        // Arrange
        var message = new CardOrderApplicationEvent
        {
            UserId = "rabbitmqUserId",
            CardCode = 100,
            ApplicationDate = new DateTime(2024, 12, 12)
        };
        _pactVerifier.WithMessages(scenarios =>
            {
                scenarios.Add($"{ComType}: CardOrderApplicationEvent handled", builder =>
                {
                    var metadata = new Dictionary<string, string>
                    {
                        {"exchangeName", "SpecialExchangeName"},
                        {"routingKey", "super-routing-key"}
                    };

                    builder.WithMetadata(metadata).WithContent(() => message);
                });
            }, _jsonSerializerOptions)
            .WithFileSource(new FileInfo(@"..\..\..\pacts\Demo.Consumer-Demo.Provider.json"))
            
            // Act && Assert
            .WithFilter(ComType)
            .Verify();
    }

    public void Dispose()
    {
        _pactVerifier?.Dispose();
    }
}