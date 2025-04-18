using BackPack.Dependency.Library.Responses;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ITenantService
    {
        Task<BaseResponse> UpdateTenantAsync(UpdateTenantRequest request);

        Task<BaseResponse> DeleteTenantAsync(DeleteTenantRequest request);       
        
    }
}
