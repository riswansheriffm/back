
using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetTenantByDomainIdRepository
    {
        Task<GetTenantByTenantNameResponse> GetTenantDBConnection(int domainId);
    }
}
