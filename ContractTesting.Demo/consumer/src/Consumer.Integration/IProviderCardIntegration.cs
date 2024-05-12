using Consumer.Integration.ProviderContracts.V1;

namespace Consumer.Integration;

public interface IProviderCardIntegration
{
    Task<UserCardAccountsDto?> GetCardAccountInfo(string userId);
}