#nullable disable
namespace Consumer.Domain.Models.V1;

public class CardsScreenResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CardAccountInfoResponse[] AccountsInfo { get; set; }
}