namespace Provider.Contracts.Models;

public class CardOrderApplicationEvent
{
    /// <summary> Идентификатор пользователя /// </summary>
    public string UserId { get; set; }
    /// <summary> Код карточного продукта /// </summary>
    public string CardCode { get; set; }
    /// <summary> Дата оформления заявления /// </summary>
    public DateTime ApplicationDate { get; set; }
}