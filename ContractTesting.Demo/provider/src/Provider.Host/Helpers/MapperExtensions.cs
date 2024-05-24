using Provider.Contracts.Models;
using Provider.Domain.Models;

namespace Provider.Host.Helpers;

public static class MapperExtensions
{
    public static UserCardAccountsResponse MapUserCardAccounts(UserCardAccounts userCardAccounts) =>
        new()
        {
            ClientFullName = userCardAccounts.UserFullName,
            Accounts = userCardAccounts.Accounts.Select(account => new CardAccountInfoResponse
            {
                Id = account.Id,
                OpenDate = account.OpenDate,
                CloseDate = account.CloseDate,
                Cards = account.Cards.Select(MapCardInfo).ToArray()
            }).ToArray(),
        };

    public static CardInfoResponse MapCardInfo(CardInfo cardInfo) =>
        new()
        {
            Id = cardInfo.Id,
            OpenDate = cardInfo.OpenDate,
            CloseDate = cardInfo.CloseDate,
            ExpiryDate = cardInfo.ExpiryDate,
            Balance = cardInfo.Balance,
            IsNamed = cardInfo.IsNamed,
            State = cardInfo.State
        };
}