using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetTenantByTenantIDRepository
    {
        Task<GetTenantByTenantNameResponse> GetTenantDBConnection(Guid tenantID);
    }
}
