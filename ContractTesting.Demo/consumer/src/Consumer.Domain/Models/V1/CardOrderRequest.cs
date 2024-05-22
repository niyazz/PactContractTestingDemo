namespace Consumer.Domain.Models.V1;

public class CardOrderRequest
{
    /// <summary> Идентификатор карточного счёта </summary>
    public string AccountId { get; set; }
    /// <summary> Признак именованности карты </summary>
    public bool IsNamed { get; set; }
}