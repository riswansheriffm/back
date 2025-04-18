using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface ICreateTenantRepository
    {
        Task<BaseResponse> CreateTenantAsync(CreateTenantRequest request);
    }
}
