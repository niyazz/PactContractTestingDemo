namespace Consumer.Integration.ProviderContracts.V1;
public class CardAccountDto
{
    public string Id { get; set; }
    public CardDto[] Cards { get; set; }
}