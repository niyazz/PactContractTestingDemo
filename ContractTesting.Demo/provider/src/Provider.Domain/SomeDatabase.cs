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
        {"userId1", "Марина Маринина"},
        {"userId2", "Петр Петров"}
    };
    private static Dictionary<string, CardAccountInfo[]> UsersCardAccounts = new()
    {
        {
            "userId1", new[]
            {
                new CardAccountInfo
                {
                    Id = "acidef8ef642-3cab-4f70-9c12-c9757e698ad1",
                    OpenDate = new DateTime(2024, 02, 14),
                    CloseDate = null,
                    Cards = new[]
                    {
                        new CardInfo
                        {
                            Id = "cdid897d676b-f5b7-496d-99cb-1d7d1b71a10d",
                            OpenDate = new DateTime(2024, 02, 14),
                            CloseDate = null,
                            ExpiryDate = new DateTime(2027, 02, 14),
                            IsNamed = true,
                            Balance = 10000m,
                            State = ActiveState
                        },
                        new CardInfo
                        {
                            Id = "cdidcae8d712-a99c-4f05-8d88-890a78d99bda",
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
                    Id = "acid6c2960ad-d688-40b0-975f-3c4bd524c4dc",
                    OpenDate = new DateTime(2022, 12, 31),
                    CloseDate = null,
                    Cards = new[]
                    {
                        new CardInfo
                        {
                            Id = "cdida42ddc9f-9480-4083-82e2-0a7e3ab9ed6c",
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
                    Id = "acid4f35ef7e-687b-44da-9e15-e091125ae880",
                    OpenDate = new DateTime(2021, 02, 23),
                    CloseDate = null,
                    Cards = new[]
                    {
                        new CardInfo
                        {
                            Id = "cdidd2cd51a8-2a80-4fdf-aaaa-7ac8bd9c5344",
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