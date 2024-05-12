using System;
using Provider.Domain.Models;

namespace Provider.ContractTests;

public static class DataForTests
{
    public static UserCardAccounts SuccessResult = new ()
    {
        UserFullName = "Иван Иванов",
        Accounts = new[]
        {
            new CardAccountInfo
            {
                Id = "acidef8ef642-3cab-4f70-9c12-c9757e698ad1",
                Cards = new[]
                {
                    new CardInfo
                    {
                        Id = "cdid897d676b-f5b7-496d-99cb-1d7d1b71a10d",
                        ExpiryDate = new DateTime(2027, 02, 14),
                        IsNamed = true,
                        Balance = 10000m,
                        State = "ACTIVE"
                    }
                },
            }
        }
    };
}