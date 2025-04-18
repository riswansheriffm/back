using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetTenantByTenantNameRepository
    {
        Task<GetTenantByTenantNameResponse> GetTenantDBConnection(string tenantName);
    }
}
