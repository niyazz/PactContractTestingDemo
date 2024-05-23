using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Consumer.Integration;
using Consumer.Integration.ProviderContracts.V1;
using PactNet;
using Xunit;
using Xunit.Abstractions;

namespace Consumer.ContractTests;

public class GetUserAccountsTests 
{
    private readonly IPactBuilderV4 _pactBuilder;

    public GetUserAccountsTests(ITestOutputHelper testOutputHelper)
    {
        var pact = Pact.V4(consumer: "Demo.Consumer", provider: "Demo.Provider", new PactConfig
        {
            Outputters = new [] { new PactXUnitOutput(testOutputHelper)},
            DefaultJsonSettings = new JsonSerializerOptions()
            {
               PropertyNameCaseInsensitive = true,
               PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        });
        _pactBuilder = pact.WithHttpInteractions();
    }
    
    [Fact(DisplayName = "Demo.Provider при запросе счетов клиента возвращает 200 и счета клиента, " +
                        "если клиент существует и у него есть есть счета")]
    public async Task GetUserAccounts_WhenClientExistWithAccounts_ReturnsSuccess200WithAccounts()
    {
        // Arrange
        var userIdForSuccess = "successId1";
        var expectedResponseBody = new CardDto
        {
            Id = "acid2797afe5-192f-4b08-9039-ae9d9652a9a7",
            ExpiryDate = new DateTime(2027, 02, 14),
            IsNamed = true,
            Balance = 0,
            State = "PENDING"
        };
        
        _pactBuilder.UponReceiving("GET - /api/provider/cards/accounts/{userId} - 200 - body")
            .WithRequest(HttpMethod.Get, $"/api/provider/cards/accounts/{userIdForSuccess}")
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
            var actualResponseBody = await contractIntegration.GetCardAccountInfo(userIdForSuccess);
            
            // Assert
            Assert.NotNull(actualResponseBody);
        });
    }
    
    [Fact(DisplayName = "Demo.Provider при запросе счетов клиента возвращает 404, " +
                        "если клиент не существует")]
    public async Task GetUserAccounts_WhenClientNotExist_ReturnsFailure404()
    {
        // Arrange
        var userIdForFailure = "failureId1"; ;

        _pactBuilder.UponReceiving("GET - /api/provider/cards/accounts/{userId} - 404 - no body")
            .WithRequest(HttpMethod.Get, $"/api/provider/cards/accounts/{userIdForFailure}")
            .WillRespond()
            .WithStatus(HttpStatusCode.NotFound);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = ctx.MockServerUri;
            var contractIntegration = new ProviderCardIntegration(httpClient);
            
            // Act
            var actualResponseBody = await contractIntegration.GetCardAccountInfo(userIdForFailure);
            
            // Assert
            Assert.Null(actualResponseBody);
        });
    }
}