using BackPack.Library.Requests.Token;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface ITokenRepository
    {
        Task<int> CreateRefreshToken(TokenRequest request);
    }
}
 