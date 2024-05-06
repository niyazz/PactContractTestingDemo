namespace Provider.Domain.Models;
public class CardAccountInfo
{
    public string Id { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public CardInfo[] Cards { get; set; }
}