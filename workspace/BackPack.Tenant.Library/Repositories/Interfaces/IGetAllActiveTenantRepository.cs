using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetAllActiveTenantRepository
    {
        Task<GetAllActiveDomainsResponse> GetAllActiveTenantsAsync();
    }
}
