namespace Provider.Contracts.Models;

public class CardAccountInfoResponse
{
    /// <summary>
    /// Идентификатор карточного счета
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Дата открытия карточного счета
    /// </summary>
    public DateTime OpenDate { get; set; }
    /// <summary>
    /// Дата закрытия карточного счета
    /// </summary>
    public DateTime? CloseDate { get; set; }
    /// <summary>
    /// Данные о клиенте
    /// </summary>
    public ClientInfoResponse ClientInfo { get; set; }
    /// <summary>
    /// Карты, привязанные к счету
    /// </summary>
    public CardInfoResponse[] Cards { get; set; }
}