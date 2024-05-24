﻿using System;
using Consumer.Integration.ProviderContracts.V1;

namespace Consumer.ContractTests;

public static class DataForTests
{
    public static UserCardAccountsDto UserCardAccountsSuccessResult = new()
    {
        ClientFullName = "Иван Иванов",
        Accounts = new[]
        {
            new CardAccountDto
            {
                Id = "acidef8ef642-3cab-4f70-9c12-c9757e698ad1",
                Cards = new[]
                {
                    new CardDto
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

    public static CardDto CardSuccessResult = new()
    {
        Id = "acid2797afe5-192f-4b08-9039-ae9d9652a9a7",
        ExpiryDate = new DateTime(2027, 02, 14),
        IsNamed = true,
        Balance = 0,
        State = "PENDING"
    };
}