
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface IGetTenantService
    {
        Task<GetAllDistrictResponse> GetAllTenantsAsync();

        Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveTenantsAsync();

        Task<GetAllActiveDomainsResponse> GetAllActiveTenantsAsync();

        Task<GetDomainAcceptedEvent> GetTenantAsync(int domainID, Guid tenantID);
    }
}
