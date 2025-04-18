
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ICreateTenantService
    {
        Task<BaseResponse> CreateTenantAsync(CreateTenantRequest request);
    }
}
