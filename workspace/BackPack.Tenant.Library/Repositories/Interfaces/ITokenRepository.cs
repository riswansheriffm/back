using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task<int> CreateRefreshToken(TokenRequest request);
    }
}
