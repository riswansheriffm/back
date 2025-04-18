
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface ILoginService
    {
        Task<UserLoginResponse> CreateUserTokenAsync(LoginRequest request, string userType);

        Task<RefreshTokenResponse> CreateRefreshTokenAsync(int UserID, string UserType, int DomainID, string UserRole);
    }
}
