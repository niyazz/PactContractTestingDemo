using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Consumer.Integration;
using Consumer.Integration.ProviderContracts.V1;
using PactHelper;
using PactNet;
using PactNet.Matchers;
using Xunit;
using Xunit.Abstractions;

namespace Consumer.ContractTests.Rest;

public class OrderCardTests : IClassFixture<PactBrokerFixture>
{
    private readonly IPactBuilderV4 _pactBuilder;
    private const string ComType = "REST";

    public OrderCardTests(ITestOutputHelper testOutputHelper, PactBrokerFixture brokerFixture)
    {
        var pact = Pact.V4(consumer: "Demo.Consumer", provider: "Demo.Provider", new PactConfig
        {
            Outputters = new[] {new PactXUnitOutput(testOutputHelper)},
            DefaultJsonSettings = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        });
        brokerFixture.PactInfo = pact;
        brokerFixture.ConsumerVersion = Assembly.GetAssembly(typeof(UserCardAccountsDto))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion!;
        _pactBuilder = pact.WithHttpInteractions();
    }

    [Fact(DisplayName = "Demo.Provider при заказе карты возвращает 200 и модель карты, " +
                        "если клиент существует и счёт найден")]
    public async Task OrderCardTests_WhenClientExistWithAccount_ReturnsSuccess200WithCard()
    {
        // Arrange
        var userIdForSuccess = "successId1";
        var accountIdForSuccess = "accountId";
        var actualRequestBody = new CreateCardOrderDto {IsNamed = true};
        var expectedResponseBody = new
        {
            Id = Match.Type(string.Empty),
            ExpiryDate = Match.Type(new DateTime(2027, 02, 14)),
            IsNamed = true,
            Balance = 0,
            State = "PENDING"
        };

        _pactBuilder.UponReceiving($"{ComType}: POST - /api/provider/cards/{{userId}}?accountId - 200 - body")
            .WithRequest(HttpMethod.Post, $"/api/provider/cards/{userIdForSuccess}")
            .WithQuery("accountId", accountIdForSuccess)
            .WithJsonBody(actualRequestBody)
            .WillRespond()
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithStatus(HttpStatusCode.OK)
            .WithJsonBody(expectedResponseBody);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = ctx.MockServerUri;
            var contractIntegration = new ProviderCardIntegration(httpClient);

            // Act
            var actualResponseBody = await contractIntegration.OrderCard(userIdForSuccess, accountIdForSuccess,
                actualRequestBody.IsNamed);

            // Assert
            Assert.NotNull(actualResponseBody);
        });
    }

    [Fact(DisplayName = "Demo.Provider при заказе карты возвращает 404, " +
                        "если клиент не существует")]
    public async Task OrderCardTests_WhenClientNotExist_ReturnsFailure404()
    {
        // Arrange
        var userIdForFailure = "failureId1";
        var accountId = "accountId";
        var actualRequestBody = new CreateCardOrderDto {IsNamed = true};

        _pactBuilder.UponReceiving($"{ComType}: POST - /api/provider/cards/{{userId}}?accountId - 404 - no body")
            .WithRequest(HttpMethod.Post, $"/api/provider/cards/{userIdForFailure}")
            .WithQuery("accountId", accountId)
            .WithJsonBody(actualRequestBody)
            .WillRespond()
            .WithStatus(HttpStatusCode.NotFound);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = ctx.MockServerUri;
            var contractIntegration = new ProviderCardIntegration(httpClient);

            // Act
            var actualResponseBody =
                await contractIntegration.OrderCard(userIdForFailure, accountId, actualRequestBody.IsNamed);

            // Assert
            Assert.Null(actualResponseBody);
        });
    }
}