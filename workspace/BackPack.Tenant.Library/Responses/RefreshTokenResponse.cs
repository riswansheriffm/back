
using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Responses
{
    public class RefreshTokenResponse : ReadBaseResponse
    {
        public TokenResponse Token { get; set; } = new TokenResponse();
    }
}
