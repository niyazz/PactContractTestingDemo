using System;
using System.Text.Json;
using System.Threading.Tasks;
using PactNet;
using PactNet.Matchers;
using Provider.Contracts.Models;
using Xunit;
using Xunit.Abstractions;

namespace Provider.ContractTests.RabbitMq.Consumer;

public class ContractWithConsumerTests
{
    private readonly IMessagePactBuilderV4 _pactBuilder;
    private const string ComType = "RABBITMQ";

    public ContractWithConsumerTests(ITestOutputHelper testOutputHelper) 
    {
        _pactBuilder = Pact.V4(consumer: "Demo.Consumer", provider: "Demo.Provider", new PactConfig
        {
            Outputters = new [] { new PactXUnitOutput(testOutputHelper)},
            DefaultJsonSettings = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        }).WithMessageInteractions();
    }

    [Fact(DisplayName = "RabbitMq контракты с Demo.Consumer соблюдаются")]
    public async Task Verify_RabbitMqDemoConsumerContacts()
    {
        // Arrange
        var message = new
        {
            UserId = Match.Type("rabbitmqUserId"),
            CardCode = Match.Integer(100),
            ApplicationDate = new DateTime(2024, 12, 12)
        };

        var mess = new CardOrderApplicationEvent
        {
            UserId = "wqe",
            CardCode = 123,
            ApplicationDate = new DateTime(2024, 12, 12) 
        };
        
        await _pactBuilder
            .ExpectsToReceive($"{ComType}: CardOrderApplicationEvent handled")
            .WithMetadata("exchangeName", "SpecialExchangeName")
            .WithMetadata("routingKey", "super-routing-key")
            .WithJsonContent(mess)
            
            // Act
            .VerifyAsync<CardOrderApplicationEvent>(async msg =>
            {
                
                // Assert
                // при желании здесь можно вызвать handler и проверить результат
                // Assert.Equal(message.Value.UserId, msg.UserId);
                // Assert.Equal(message.CardCode.Value.CardCode, msg.CardCode);
                // Assert.Equal(message.ApplicationDate, msg.ApplicationDate);
            });
    }
}