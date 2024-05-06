using Provider.Domain.Models;

namespace Provider.Domain;

public static class SomeDatabase
{
    private const string ActiveState = "ACTIVE";
    private const string BlockedState = "BLOCKED";
    private const string FrozenState = "FROZEN";


    public static (string, CardAccountInfo[])? GetData(string userId)
    {
        if (UserData.TryGetValue(userId, out var userData) && UsersCardAccounts.TryGetValue(userId, out var userAccounts))
        {
            return (userData, userAccounts);
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
                    Id = Guid.NewGuid().ToString(),
                    OpenDate = new DateTime(2024, 02, 14),
                    CloseDate = null,
                    Cards = new[]
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
                    Cards = new[]
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
                    Cards = new[]
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