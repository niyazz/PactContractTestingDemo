namespace Provider.Contracts.Models;

public class CardInfoResponse
{
    public string Id { get; set; }
    public bool IsNamed { get; set; }
    public decimal Balance { get; set; }
    public string State { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime ExpiryDate { get; set; } 
}