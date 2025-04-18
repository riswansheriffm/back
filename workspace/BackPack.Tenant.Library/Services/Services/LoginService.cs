using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Tenant.Library.Services.Services
{
    public class LoginService(
        ILoginRepository loginRepository,
        IGlobalRepository globalRepository,
        ITokenService tokenService, 
        IConfiguration configuration
        ) : ILoginService
    {
        #region CreateUserTokenAsync
        public async Task<UserLoginResponse> CreateUserTokenAsync(LoginRequest request, string userType)
        {
            var result = await loginRepository.UserForLogin(request: request, userType: userType);
            var user = result.Item1;
            int PasswordCount = result.Item2;

            #region Locked response
            if (PasswordCount > 0)
            {
                return new UserLoginResponse()
                {
                    MessageID = CommonMessage.LockedID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionType = CommonMessage.ExceptionTypeNormal,
                    StatusMessage = UserMessage.LockedAccount
                };
            }
            #endregion

            #region User not found response
            if (!user.Any())
            {
                return new UserLoginResponse()
                {
                    MessageID = CommonMessage.NotFoundID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionType = CommonMessage.ExceptionTypeNormal,
                    StatusMessage = UserMessage.UserNotFound
                };
            }
            #endregion

            #region Token creation
            if (user.Any())
            {
                UserLoginData userData = new();
                var data = user.First();
                int userID = data.ID;
                var passwordHash = Hash.HashUsingPbkdf2(request.Password, Convert.FromBase64String(data.PasswordSalt));

                #region Invalid password response
                if (data.Password != passwordHash)
                {
                    PasswordCount = await loginRepository.UpdateWrongPassword(request: request, userType: userType);

                    if (PasswordCount > 0)
                    {
                        return new UserLoginResponse()
                        {
                            MessageID = CommonMessage.LockedID,
                            Success = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            ExceptionType = CommonMessage.ExceptionTypeNormal,
                            StatusMessage = UserMessage.LockedAccount
                        };
                    }

                    return new UserLoginResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        ExceptionType = CommonMessage.ExceptionTypeNormal,
                        StatusMessage = UserMessage.InvalidPassword
                    };
                }
                #endregion

                #region User data                        
                userData.DomainID = data.DomainID;
                userData.DomainName = data.DomainName;
                userData.LoginID = data.ID;
                userData.LoginName = data.LoginName;
                userData.FirstName = data.FName;
                userData.LastName = data.LName;
                userData.FullName = data.FullName;
                userData.UserRole = data.UserRole;
                #endregion

                await loginRepository.ResetWrongPassword(request: request, userType: userType);

                Aes256Helper aes256Helper = new(configuration);
                string issuer = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Issuer).Value!);
                string audience = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Audience).Value!);
                string key = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Key).Value!);
                int tokenExpirationInMinutes = Convert.ToInt32(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.TokenExpirationInMinutes).Value!);
                int refreshTokenExpirationInDays = Convert.ToInt32(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.RefreshTokenExpirationInDays).Value!);

                #region Token response
                TokenRequest tokenRequest = new()
                {
                    UserID = userID,
                    TokenUserID = aes256Helper.Aes256Encryption(userID.ToString()),
                    Issuer = issuer,
                    Audience = audience,
                    Key = key,
                    TokenExpirationInMinutes = tokenExpirationInMinutes,
                    TokenUserType = aes256Helper.Aes256Encryption(userType),
                    UserType = userType,
                    TokenLoginName = aes256Helper.Aes256Encryption(data.LoginName),
                    TenantID = "",
                    TenantName = "",
                    DBConnection = "",
                    RefreshTokenExpirationInDays = refreshTokenExpirationInDays,
                    TokenDomainID = aes256Helper.Aes256Encryption(data.DomainID.ToString()),
                    UserRole = aes256Helper.Aes256Encryption(data.UserRole.ToString())
                };

                var token = await tokenService.GenerateTokensAsync(tokenRequest);

                TokenResponse tokenResponse = new()
                {
                    TenantID = GlobalApplicationProperty.TenantID,
                    UserID = userID,
                    UserType = userType,
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2,
                    TokenType = ServiceConstant.TokenType,
                    TokenExpiresIn = tokenExpirationInMinutes * 60,
                    RefreshTokenExpiresIn = refreshTokenExpirationInDays * 24 * 60 * 60
                };

                return new UserLoginResponse()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Token = tokenResponse,
                    Data = userData
                };
                #endregion
            }
            #endregion

            #region Response
            return new UserLoginResponse()
            {
                MessageID = CommonMessage.ErrorID,
                Success = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                ExceptionType = CommonMessage.ExceptionTypeFail,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            };
            #endregion
        }
        #endregion

        #region CreateRefreshTokenAsync
        public async Task<RefreshTokenResponse> CreateRefreshTokenAsync(int UserID, string UserType, string UserRole)
        {
            Aes256Helper aes256Helper = new(configuration);
            string issuer = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Issuer).Value!);
            string audience = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Audience).Value!);
            string key = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.Key).Value!);
            int tokenExpirationInMinutes = Convert.ToInt32(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.TokenExpirationInMinutes).Value!);
            int refreshTokenExpirationInDays = Convert.ToInt32(configuration.GetSection(ServiceConstant.JwtSettings).GetSection(ServiceConstant.RefreshTokenExpirationInDays).Value!);
            string loginName = await globalRepository.GetLoginNameByID(userID: UserID, userType: UserType);

            TokenRequest tokenRequest = new()
            {
                UserID = UserID,
                TokenUserID = aes256Helper.Aes256Encryption(UserID.ToString()),
                Issuer = issuer,
                Audience = audience,
                Key = key,
                TokenExpirationInMinutes = tokenExpirationInMinutes,
                TokenUserType = aes256Helper.Aes256Encryption(UserType),
                UserType = UserType,
                TokenLoginName = aes256Helper.Aes256Encryption(loginName),
                TenantID = "",
                TenantName = "",
                DBConnection = "",
                RefreshTokenExpirationInDays = refreshTokenExpirationInDays,
                TokenDomainID = aes256Helper.Aes256Encryption("0"),
                UserRole = aes256Helper.Aes256Encryption(UserRole.ToString())
            };

            var token = await tokenService.GenerateTokensAsync(tokenRequest);

            TokenResponse tokenResponse = new()
            {
                TenantID = GlobalApplicationProperty.TenantID,
                UserID = UserID,
                UserType = UserType,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                TokenType = ServiceConstant.TokenType,
                TokenExpiresIn = tokenExpirationInMinutes * 60,
                RefreshTokenExpiresIn = refreshTokenExpirationInDays * 24 * 60 * 60
            };

            return new RefreshTokenResponse()
            {
                MessageID = CommonMessage.SuccessID,
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Token = tokenResponse
            };
        }
        #endregion
    }
}
