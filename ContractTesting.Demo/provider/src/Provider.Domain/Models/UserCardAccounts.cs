namespace Provider.Domain.Models;

public class UserCardAccounts
{
    public string UserFullName { get; set; }
    public CardAccountInfo[] Accounts { get; set; }
}