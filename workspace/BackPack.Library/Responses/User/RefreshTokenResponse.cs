using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class RefreshTokenResponse : ReadBaseResponse
    {
        public TokenResponse Token { get; set; } = new TokenResponse();
    }
}
