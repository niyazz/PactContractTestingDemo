using Provider.Domain.Models;

namespace Provider.Domain;

public static class SomeDatabase
{
    private const string ActiveState = "ACTIVE";
    private const string BlockedState = "BLOCKED";
    private const string FrozenState = "FROZEN";
    private const string PendingState = "PENDING";


    public static (string, CardAccountInfo[])? GetData(string userId)
    {
        if (UserData.TryGetValue(userId, out var userData) && UsersCardAccounts.TryGetValue(userId, out var userAccounts))
        {
            return (userData, userAccounts);
        }
        
        return null;
    }
    
    public static CardInfo? AddCard(string userId, string accountId, bool isNamed)
    {
        if (UserData.TryGetValue(userId, out var userData) && UsersCardAccounts.TryGetValue(userId, out var userAccounts))
        {
            var now = DateTime.Now;
            var account = userAccounts.FirstOrDefault(a => a.Id == accountId);
            if (account != null)
            {
                var card = new CardInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    Balance = 0,
                    ExpiryDate = now + TimeSpan.FromDays(365 * 3),
                    OpenDate = now,
                    CloseDate = null,
                    IsNamed = isNamed,
                    State = PendingState
                };
                
                account.Cards.Add(card);
                return card;
            }
        }
        
        return null;
    }
    
    private static Dictionary<string, string> UserData = new()
    {
        {"userId1", "Иван_Иванов"},
        {"userId2", "Петр_Петров"}
    };
    private static Dictionary<string, CardAccountInfo[]> UsersCardAccounts = new()
    {
        {
            "userId1", new[]
            {
                new CardAccountInfo
                {
                    Id = "bc94da79-a290-4d27-96e1-d6cc5453be68",
                    OpenDate = new DateTime(2024, 02, 14),
                    CloseDate = null,
                    Cards = new()
                    {
                        new CardInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            OpenDate = new DateTime(2024, 02, 14),
                            CloseDate = null,
                            ExpiryDate = new DateTime(2027, 02, 14),
                            IsNamed = true,
                            Balance = 10000m,
                            State = ActiveState
                        },
                        new CardInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            OpenDate = new DateTime(2024, 03, 8),
                            CloseDate = null,
                            ExpiryDate = new DateTime(2027, 03, 8),
                            IsNamed = true,
                            Balance = -10.6m,
                            State = FrozenState
                        }
                    },
                },
                new CardAccountInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    OpenDate = new DateTime(2022, 12, 31),
                    CloseDate = null,
                    Cards = new()
                    {
                        new CardInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            OpenDate = new DateTime(2022, 12, 31),
                            CloseDate = null,
                            ExpiryDate = new DateTime(2025, 12, 31),
                            IsNamed = true,
                            Balance = 500m,
                            State = ActiveState
                        },
                    },
                }
            }
        },
        {
            "userId2", new[]
            {
                new CardAccountInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    OpenDate = new DateTime(2021, 02, 23),
                    CloseDate = null,
                    Cards = new()
                    {
                        new CardInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            OpenDate = new DateTime(2021, 02, 23),
                            CloseDate = null,
                            ExpiryDate = new DateTime(2024, 02, 23),
                            IsNamed = true,
                            Balance = 0m,
                            State = BlockedState
                        }
                    }
                }
            }
        }
    };
}