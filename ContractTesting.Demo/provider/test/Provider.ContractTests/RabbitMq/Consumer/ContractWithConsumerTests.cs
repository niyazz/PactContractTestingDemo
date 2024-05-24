using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PactNet.Verifier;
using Provider.Contracts.Models;
using Xunit;
using Xunit.Abstractions;

namespace Provider.ContractTests.RabbitMq.Consumer;
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
    
    [Fact(DisplayName = "RabbitMq контракты с потребителем Demo.Consumer соблюдаются")]
    public void Verify_RabbitMqDemoConsumerContacts()
    {
        // Arrange
        var userId = "rabbitUserId";
        var cardCode = 100;
        var metadata = new Dictionary<string, string>
        {
            {"exchangeName", "SpecialExchangeName"},
            {"routingKey", "super-routing-key"}
        };
        _pactVerifier.WithMessages(scenarios =>
            {
                scenarios.Add($"{ComType}: CardOrderSatisfiedEvent with push", builder =>
                {
                    builder.WithMetadata(metadata).WithContent(() => new CardOrderSatisfiedEvent
                    {
                        UserId = userId, CardCode = cardCode, ShouldBeNotified = true
                    });
                });
                scenarios.Add($"{ComType}: CardOrderSatisfiedEvent no push", builder =>
                {
                    builder.WithMetadata(metadata).WithContent(() => new CardOrderSatisfiedEvent
                    {
                        UserId = userId, CardCode = cardCode, ShouldBeNotified = false
                    });
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