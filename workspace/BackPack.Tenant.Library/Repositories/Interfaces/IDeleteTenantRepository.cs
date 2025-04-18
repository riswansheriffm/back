using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IDeleteTenantRepository
    {
        Task<BaseResponse> DeleteTenantAsync(DeleteTenantRequest request);
    }
}
