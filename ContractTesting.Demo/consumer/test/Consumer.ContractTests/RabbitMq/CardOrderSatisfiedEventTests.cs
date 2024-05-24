using System.Text.Json;
using Consumer.Domain.Models.V1;
using PactHelper;
using PactNet;
using Xunit;
using Xunit.Abstractions;
using Match = PactNet.Matchers.Match;

namespace Consumer.ContractTests.RabbitMq;

public class CardOrderSatisfiedEventTests : IClassFixture<PactBrokerFixture>
{
    private readonly IMessagePactBuilderV4 _pactBuilder;
    private const string ComType = "RABBITMQ";

    public CardOrderSatisfiedEventTests(ITestOutputHelper testOutputHelper, PactBrokerFixture brokerFixture)
    {
        var pact = Pact.V4(consumer: "Demo.Consumer", provider: "Demo.Provider", new PactConfig
        {
            Outputters = new[] {new PactXUnitOutput(testOutputHelper)},
            DefaultJsonSettings = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        });
        _pactBuilder = pact.WithMessageInteractions();
        brokerFixture.PactInfo = pact;
        brokerFixture.ConsumerVersion = Assembly.GetAssembly(typeof(CardOrderSatisfiedEvent))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion!;
    }

    [Fact(DisplayName = "Demo.Provider присылает корректный контракт и пуш отправляется," +
                        " когда получено событие и необходимо уведомление клиента")]
    public void CardOrderSatisfiedEvent_WhenModelCorrectAndShouldBePushed_SendsPush()
    {
        // Arrange
        var message = new
        {
            UserId = Match.Type("rabbitmqUserId"),
            CardCode = Match.Integer(100),
            ShouldBeNotified = true
        };

        _pactBuilder
            .ExpectsToReceive($"{ComType}: CardOrderSatisfiedEvent with push")
            .WithMetadata("exchangeName", "SpecialExchangeName")
            .WithMetadata("routingKey", "super-routing-key")
            .WithJsonContent(message)

            // Act
            .Verify<CardOrderSatisfiedEvent>(msg =>
            {
                // Assert
                // здесь можно вызвать IConsumer.Handle и проверить логику работы обработчика
                //_consumerCardService.Verify(x => x.PushUser(msg), Times.Once);
            });
    }
    
    [Fact(DisplayName = "Demo.Provider присылает корректный контракт и пуш не отправляется," +
                        " когда получено событие и не нужно уведомление клиента")]
    public void CardOrderSatisfiedEvent_WhenModelCorrectAndShouldNotBePushed_DontSendPush()
    {
        // Arrange
        var message = new
        {
            UserId = Match.Type("rabbitmqUserId"),
            CardCode = Match.Integer(100),
            ShouldBeNotified = false
        };

        _pactBuilder
            .ExpectsToReceive($"{ComType}: CardOrderSatisfiedEvent no push")
            .WithMetadata("exchangeName", "SpecialExchangeName")
            .WithMetadata("routingKey", "super-routing-key")
            .WithJsonContent(message)

            // Act
            .Verify<CardOrderSatisfiedEvent>(msg =>
            {
                // Assert
                // здесь можно вызвать IConsumer.Handle и проверить логику работы обработчика
                //_consumerCardService.Verify(x => x.PushUser(msg), Times.Never);
            });
    }
}