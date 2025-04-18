using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetAllPublicActiveTenantRepository
    {
        Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveTenantsAsync();
    }
}
