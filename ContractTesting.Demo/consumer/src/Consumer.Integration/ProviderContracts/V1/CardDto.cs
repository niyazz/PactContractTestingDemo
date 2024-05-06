namespace Consumer.Integration.ProviderContracts.V1;
public class CardDto
{
    public string Id { get; set; }
    public bool IsNamed { get; set; }
    public decimal Balance { get; set; }
    public string State { get; set; }
    public DateTime ExpiryDate { get; set; }
}