using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ILoginService
    {
        Task<UserLoginResponse> CreateUserTokenAsync(LoginRequest request, string userType);

        Task<RefreshTokenResponse> CreateRefreshTokenAsync(int UserID, string UserType, string UserRole);
    }
}
