using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface IValidateRefreshTokenRepository
    {
        Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request);
    }
}
