namespace Provider.Contracts.Models;
public class CardInfoResponse
{
    ///<summary> Идентификатор карты </summary>
    public string Id { get; set; }
    ///<summary> Признак именнованости карты </summary>
    public bool IsNamed { get; set; }
    ///<summary> Баланс карты </summary>
    public decimal Balance { get; set; }
    ///<summary> Статус карты </summary>
    public string State { get; set; }
    ///<summary> Дата открытия карты </summary>
    public DateTime OpenDate { get; set; }
    ///<summary> Дата закрытия карты </summary>
    public DateTime? CloseDate { get; set; }
    ///<summary> Срок действия карты </summary>
    public DateTime ExpiryDate { get; set; } 
}