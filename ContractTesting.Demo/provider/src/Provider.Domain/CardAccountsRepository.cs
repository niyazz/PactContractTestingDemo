using Provider.Domain.Models;

namespace Provider.Domain;

public class CardAccountsRepository : ICardAccountsRepository
{
    public async Task<UserCardAccounts?> GetCardAccountsByUserId(string userId)
    {
        await Task.Delay(1000);
        var dbResult = SomeDatabase.GetData(userId);

        return dbResult != null
            ? new UserCardAccounts
            {
                UserFullName = dbResult.Value.Item1,
                Accounts = dbResult.Value.Item2
            }
            : null;
    }
    
    public async Task<CardInfo?> AddCard(string userId, string accountId, bool isNamed)
    {
        await Task.Delay(1000);
        var dbResult = SomeDatabase.AddCard(userId, accountId, isNamed);

        return dbResult;
    }
}