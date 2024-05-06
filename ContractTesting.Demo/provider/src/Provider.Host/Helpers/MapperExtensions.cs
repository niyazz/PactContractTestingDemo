using Provider.Contracts.Models;
using Provider.Domain.Models;

namespace Provider.Host.Helpers;

public static class MapperExtensions
{
    public static UserCardAccountsResponse MapDomainModelToContract(UserCardAccounts userCardAccounts) =>

        new ()
        {
            ClientFullName = userCardAccounts.UserFullName,
            Accounts = userCardAccounts.Accounts.Select(account => new CardAccountInfoResponse
            {
                Id = account.Id,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate,
                Cards = account.Cards.Select(card => new CardInfoResponse
                {
                    Id = card.Id,
                    OpenDate = card.OpenDate,
                    CloseDate = card.CloseDate,
                    ExpiryDate = card.ExpiryDate,
                    Balance = card.Balance,
                    IsNamed = card.IsNamed,
                    State = card.State
                }).ToArray()
            }).ToArray(),
        };


}