namespace Consumer.Integration.ProviderContracts.V1;
public class UserCardAccountsDto
{
    public string ClientFullName { get; set; }
    public CardAccountDto[] Accounts { get; set; }
}