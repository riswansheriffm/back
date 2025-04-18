using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.Token;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IValidateRefreshTokenRepository
    {
        Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request);
    }
}
