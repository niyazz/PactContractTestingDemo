namespace Consumer.Domain.Models.V1;

public class CardAccountInfoResponse
{
    /// <summary>
    /// Идентификатор карточного счета
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Карты, привязанные к счету
    /// </summary>
    public CardInfoResponse[] Cards { get; set; }
}