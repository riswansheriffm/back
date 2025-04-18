using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface ITenantConsumerRepository
    {
        Task<BaseResponse> DeleteTenantByConsumerRejectedAsync(Guid tenantID);

        Task<BaseResponse> UpdateTenantByConsumerAcceptedAsync(Guid tenantID, int domainID);
    }
}
