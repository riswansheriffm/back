using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IGetAllTenantRepository
    {
        Task<GetAllDistrictResponse> GetAllTenantsAsync();
    }
}
