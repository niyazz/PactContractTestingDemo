#nullable disable
namespace Consumer.Domain.Models.V1;

public class CardsScreenResponse
{
    ///<summary> Заголовок для клиента </summary>
    public string Title { get; set; }
    /// <summary> Подробности </summary>
    public string Description { get; set; }
    /// <summary> Карточные счета </summary>
    public CardAccountInfoResponse[] AccountsInfo { get; set; }
}