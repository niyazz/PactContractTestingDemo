namespace Consumer.Integration.ProviderContracts.V1;

public class CardOrderApplicationEvent
{
    /// <summary> Идентификатор пользователя /// </summary>
    public string UserId { get; set; }
    /// <summary> Код карточного продукта /// </summary>
    public int CardCode { get; set; }
    /// <summary> Дата оформления заявления /// </summary>
    public DateTime ApplicationDate { get; set; }
}