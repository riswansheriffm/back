using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.Token;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses;
using BackPack.Library.Services.Interfaces.User;
using KnomadixInfrastructure.AES256;
using KnomadixInfrastructure.JWT;
using System.Data.Common;
using System.Net;

namespace BackPack.Library.Services.Services.User
{
    public class TokenService(
        ITokenRepository tokenRepository, 
        IValidateRefreshTokenRepository validateRefreshTokenRepository
        ) : ITokenService
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
