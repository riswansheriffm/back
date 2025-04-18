using BackPack.Library.Requests.Token;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface ITokenService
    {
        Task<Tuple<string, string>> GenerateTokensAsync(TokenRequest request);

        Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request);
    }
}
