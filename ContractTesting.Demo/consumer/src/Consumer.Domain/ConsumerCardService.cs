using Consumer.Domain.Models.V1;
using Consumer.Domain.Utils;
using Consumer.Integration;

namespace Consumer.Domain;

public class ConsumerCardService
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
        
        var mappedAccountsInfo = MapUserCardAccountsDto(accountsInfo);
        var availableCardsCount = mappedAccountsInfo.SelectMany(x => x.Cards).Count(x => x.IsAvailable);

        return Result.CreateSuccess(new CardsScreenResponse
        {
            Title = "Ваши карты",
            Description = $"Сейчас у вас {availableCardsCount} доступных для пользования карт",
            AccountsInfo = mappedAccountsInfo
        });
    }
    
    public async Task<Result<CardsScreenResponse>> OrderCard(string userId, CardOrderRequest request)
    {
        var cardInfo = await _providerCardIntegration.OrderCard(userId, request.AccountId, request.IsNamed);

        if (cardInfo == null) 
            return Result.CreateFailure<CardsScreenResponse>();
        
        return Result.CreateSuccess(new CardsScreenResponse
        {
            Title = "Ожидайте",
            Description = $"Ваша карта {cardInfo.Id} в процессе выпуска"
        });
    }

    private CardAccountInfoResponse[] MapUserCardAccountsDto(Integration.ProviderContracts.V1.UserCardAccountsDto userCardAccounts)
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