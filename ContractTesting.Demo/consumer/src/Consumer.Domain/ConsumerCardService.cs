using Consumer.Domain.Models.V1;
using Consumer.Domain.Utils;
using Consumer.Integration;

namespace Consumer.Domain;

public interface IConsumerCardService
{
    Task<Result<CardsScreenResponse>> GetCardsScreen(string userId);
    void PushUser(CardOrderSatisfiedEvent orderSatisfied);
}

public class ConsumerCardService : IConsumerCardService
{
    private readonly ProviderCardIntegration _providerCardIntegration;

    public ConsumerCardService(ProviderCardIntegration providerCardIntegration)
    {
        _providerCardIntegration = providerCardIntegration;
    }
    
    public async Task<Result<CardsScreenResponse>> GetCardsScreen(string userId)
    {
        var accountsInfo = await _providerCardIntegration.GetCardAccountInfo(userId);

        if (accountsInfo == null) 
            return Result.CreateFailure<CardsScreenResponse>();
        
        var mappedAccountsInfo = MapIntegrationToDomainModel(accountsInfo);
        var availableCardsCount = mappedAccountsInfo.SelectMany(x => x.Cards).Count(x => x.IsAvailable);

        return Result.CreateSuccess(new CardsScreenResponse
        {
            Title = "Ваши карты",
            Description = $"Сейчас у вас {availableCardsCount} доступных для пользования карт",
            AccountsInfo = mappedAccountsInfo
        });
    }

    public void PushUser(CardOrderSatisfiedEvent orderSatisfied)
    {
        Console.WriteLine($"Ваша карта готова: {orderSatisfied.UserId}, {orderSatisfied.CardCode}");
    }
    
    private CardAccountInfoResponse[] MapIntegrationToDomainModel(Integration.ProviderContracts.V1.UserCardAccountsDto userCardAccounts)
    {
        var clientName = userCardAccounts.ClientFullName;
        var clientAccounts = userCardAccounts.Accounts.Select(account => new CardAccountInfoResponse
        {
            Id = account.Id,
            Cards = account.Cards.Select(card => new CardInfoResponse
            {
                Id = card.Id,
                IsAvailable = card.State == "ACTIVE",
                Balance = card.Balance,
                ExpiryDate = card.ExpiryDate.ToString("dd.MM.yyyy"),
                HolderName = card.IsNamed
                    ? clientName.ToUpper()
                    : string.Empty,
            }).ToArray()
        }).ToArray();

        return clientAccounts;
    }
}