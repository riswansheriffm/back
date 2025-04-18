using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ITokenService
    {
        Task<Tuple<string, string>> GenerateTokensAsync(TokenRequest request);

        Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request);
    }
}
