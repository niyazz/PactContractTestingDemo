using System.Text.Json;
using System.Threading.Tasks;
using Consumer.Domain;
using Consumer.Domain.Models.V1;
using Moq;
using PactNet;
using Xunit;
using Xunit.Abstractions;
using Match = PactNet.Matchers.Match;

namespace Consumer.ContractTests.RabbitMq;

public class CardOrderSatisfiedEventTests
{
    private readonly IMessagePactBuilderV4 _pactBuilder;
    private readonly Mock<ConsumerCardService> _consumerCardService;
    private const string ComType = "RABBITMQ";

    public CardOrderSatisfiedEventTests(ITestOutputHelper testOutputHelper)
    {
        _pactBuilder = Pact.V4(consumer: "Demo.Consumer", provider: "Demo.Provider", new PactConfig
        {
            Outputters = new[] {new PactXUnitOutput(testOutputHelper)},
            DefaultJsonSettings = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        }).WithMessageInteractions();
        _consumerCardService = new Mock<ConsumerCardService>();
    }

    [Fact(DisplayName = "Demo.Provider присылает корректный контракт и пуш отправляется," +
                        " когда получено событие и необходимо уведомление клиента")]
    public async Task CardOrderSatisfiedEvent_WhenModelCorrectAndShouldBePushed_SendsPush()
    {
        // Arrange
        var message = new
        {
            UserId = Match.Type("rabbitmqUserId"),
            CardCode = Match.Integer(100),
            
        };

        await _pactBuilder
            .ExpectsToReceive($"{ComType}: CardOrderSatisfiedEvent handled")
            .WithMetadata("exchangeName", "SpecialExchangeName")
            .WithMetadata("routingKey", "super-routing-key")
            .WithJsonContent(message)

            // Act
            .VerifyAsync<CardOrderSatisfiedEvent>(async msg =>
            {

                // Assert
                _consumerCardService
                    .Verify(x => x.PushUser(It.IsAny<CardOrderSatisfiedEvent>()),
                        Times.Once);
            });
    }
    
    [Fact(DisplayName = "Demo.Provider присылает корректный контракт и пуш не отправляется," +
                        " когда получено событие и не нужно уведомление клиента")]
    public async Task CardOrderSatisfiedEvent_WhenModelCorrectAndShouldNotBePushed_SendsPush()
    {
        // Arrange
        var message = new
        {
            UserId = Match.Type("rabbitmqUserId"),
            CardCode = Match.Integer(100),
            ShouldBeNotified = true
        };

        await _pactBuilder
            .ExpectsToReceive($"{ComType}: CardOrderSatisfiedEvent handled")
            .WithMetadata("exchangeName", "SpecialExchangeName")
            .WithMetadata("routingKey", "super-routing-key")
            .WithJsonContent(message)

            // Act
            .VerifyAsync<CardOrderSatisfiedEvent>(async msg =>
            {

                // Assert
                _consumerCardService
                    .Verify(x => x.PushUser(It.IsAny<CardOrderSatisfiedEvent>()),
                        Times.Never);
            });
    }
}