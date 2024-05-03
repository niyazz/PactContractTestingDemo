namespace Consumer.Domain.Models.V1;

public class CardInfoResponse
{
    public string Id { get; set; }
    public string? HolderName { get; set; }
    public decimal Balance { get; set; }
    public bool IsAvailable { get; set; }
    public string ExpiryDate { get; set; }
}