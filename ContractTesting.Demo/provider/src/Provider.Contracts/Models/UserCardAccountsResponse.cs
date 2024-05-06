namespace Provider.Contracts.Models;

public class UserCardAccountsResponse
{
    ///<summary> Полное имя клиента </summary>
    public string ClientFullName { get; set; }
    
    ///<summary> Карточные счета клиента </summary>
    public CardAccountInfoResponse[] Accounts { get; set; }
}