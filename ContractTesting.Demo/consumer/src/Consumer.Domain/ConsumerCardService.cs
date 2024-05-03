using Consumer.Domain.Models.V1;
using Consumer.Domain.Utils;
using Consumer.Integration;
using Consumer.Integration.ProviderContracts.V1;
using CardAccountInfoResponse = Consumer.Domain.Models.V1.CardAccountInfoResponse;

namespace Consumer.Domain;

public class ConsumerCardService
{
    private readonly IProviderCardIntegration _providerCardIntegration;

    public ConsumerCardService(IProviderCardIntegration providerCardIntegration)
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

    private CardAccountInfoResponse[] MapIntegrationToDomainModel(Integration.ProviderContracts.V1.CardAccountInfoResponse[] accountsInfo)
    {
        var clientName = accountsInfo.FirstOrDefault()?.ClientInfo ?? throw new ArgumentException("У клиента должно быть ФИО");
        var clientAccounts = accountsInfo.Select(account => new CardAccountInfoResponse
        {
            Id = account.Id,
            Cards = account.Cards.Select(card => new Models.V1.CardInfoResponse
            {
                Id = card.Id,
                IsAvailable = card.State == "ACTIVE",
                Balance = card.Balance,
                ExpiryDate = card.ExpiryDate.ToString("dd.MM.yyyy"),
                HolderName = card.IsNamed
                    ? $"{clientName.LastName.ToUpper()} {clientName.Name.ToUpper()}"
                    : string.Empty,
            }).ToArray()
        }).ToArray();

        return clientAccounts;
    }
}