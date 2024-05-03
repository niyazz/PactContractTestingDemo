using Consumer.Integration.ProviderContracts.V1;

namespace Consumer.Integration;

public interface IProviderCardIntegration
{
    Task<CardAccountInfoResponse[]?> GetCardAccountInfo(string userId);
}