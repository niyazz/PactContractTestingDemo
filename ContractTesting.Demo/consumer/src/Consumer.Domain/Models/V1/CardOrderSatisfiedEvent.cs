namespace Consumer.Domain.Models.V1;
public class CardOrderSatisfiedEvent
{
    /// <summary> Идентификатор пользователя /// </summary>
    public string UserId { get; set; }
    /// <summary> Код карточного продукта /// </summary>
    public int CardCode { get; set; }
    /// <summary> Клиента нужно уведомить /// </summary>
    public bool ShouldBeNotified { get; set; }
}