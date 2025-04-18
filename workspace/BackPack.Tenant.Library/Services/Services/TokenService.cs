using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Services.Interfaces;
using KnomadixInfrastructure.AES256;
using KnomadixInfrastructure.JWT;

namespace BackPack.Tenant.Library.Services.Services
{
    public class TokenService(ITokenRepository tokenRepository, IValidateRefreshTokenRepository validateRefreshTokenRepository) : ITokenService
    {
        #region GenerateTokensAsync
        public async Task<Tuple<string, string>> GenerateTokensAsync(TokenRequest request)
        {
            var accessToken = await JWToken.GenerateAccessToken(userID: request.TokenUserID, tenantID: request.TenantID, tenantName: request.TenantName, dbConnection: request.DBConnection, userType: request.TokenUserType, loginName: request.TokenLoginName, issuer: request.Issuer, audience: request.Audience, key: request.Key, tokenExpirationInMinutes: request.TokenExpirationInMinutes, domainID: request.TokenDomainID, userRole: request.UserRole);
            var refreshToken = await JWToken.GenerateRefreshToken();
            var salt = Hash.GetSecureSalt();
            var refreshTokenHash = Hash.HashUsingPbkdf2(refreshToken, salt);
            request.TokenSalt = Convert.ToBase64String(salt);
            request.TokenHash = refreshTokenHash;

            int userCount = await tokenRepository.CreateRefreshToken(request);

            if (userCount == 0)
            {
                return new Tuple<string, string>("", "");
            }

            var token = new Tuple<string, string>(accessToken, refreshToken);

            return token;
        }
        #endregion

        #region ValidateRefreshTokenAsync
        public async Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request)
        {
            BaseResponse response = await validateRefreshTokenRepository.ValidateRefreshTokenAsync(request);

            return response;
        }
        #endregion
    }
}
