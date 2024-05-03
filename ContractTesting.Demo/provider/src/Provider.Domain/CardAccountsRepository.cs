using Provider.Domain.Models;

namespace Provider.Domain;

public class CardAccountsRepository
{
    public async Task<CardAccountInfo[]?> GetCardAccountsByUserId(string userId)
    {
        await Task.Delay(1000);
        DatabaseFakes.UsersCardAccounts.TryGetValue(userId, out var userCardAccounts);
        return userCardAccounts;
    }
}