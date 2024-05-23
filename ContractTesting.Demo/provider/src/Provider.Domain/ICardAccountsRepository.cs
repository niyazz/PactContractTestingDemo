using Provider.Domain.Models;

namespace Provider.Domain;

public interface ICardAccountsRepository
{
    Task<UserCardAccounts?> GetCardAccountsByUserId(string userId);
    Task<CardInfo?> AddCard(string userId, string accountId, bool isNamed);
}