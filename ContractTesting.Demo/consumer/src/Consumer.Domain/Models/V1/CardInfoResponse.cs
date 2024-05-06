namespace Consumer.Domain.Models.V1;

public class CardInfoResponse
{
    ///<summary> Идентификатор карты </summary>
    public string Id { get; set; }
    ///<summary> Имя держателя карты </summary>
    public string? HolderName { get; set; }
    ///<summary> Баланс карты </summary>
    public decimal Balance { get; set; }
    ///<summary> Доступность карты для пользования </summary>
    public bool IsAvailable { get; set; }
    ///<summary> Срок работы карты </summary>
    public string ExpiryDate { get; set; }
}