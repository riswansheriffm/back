using BackPack.APIGateway.Constants;
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace BackPack.APIGateway.Middleware
{
    public class JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        #region Invoke
        public async Task<BaseResponse> Invoke(HttpContext context)
        {
            #region Error log condition
            if (context.Request.Path.Value!.Contains("favico"))
            {
                await next(context);
                return new BaseResponse() { };
            }
            #endregion

            var serviceName = GlobalHelper.ServiceMethodName(context);            

            if (serviceName == ServiceConstant.ServiceStatusService)
            {
                context.Request.EnableBuffering();
                using StreamReader reader = new(context.Request.Body, Encoding.UTF8, false, 1024, true);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                await next(context);
                return new BaseResponse() { };
            }

            if (serviceName != "" && (serviceName == ServiceConstant.SuperAdminLoginService || serviceName == ServiceConstant.TeacherLoginService || serviceName == ServiceConstant.StudentLoginService || serviceName == ServiceConstant.RefreshTokenService || serviceName == ServiceConstant.ActivateUserAccountService || serviceName == ServiceConstant.ResetPasswordService || serviceName == ServiceConstant.CreatePasswordService))
            {
                context.Request.EnableBuffering();
                using StreamReader reader = new(context.Request.Body, Encoding.UTF8, false, 1024, true);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            if (serviceName != "" && !(serviceName == ServiceConstant.SuperAdminLoginService || serviceName == ServiceConstant.TeacherLoginService || serviceName == ServiceConstant.StudentLoginService || serviceName == ServiceConstant.RefreshTokenService || serviceName == ServiceConstant.ActivateUserAccountService || serviceName == ServiceConstant.ResetPasswordService || serviceName == ServiceConstant.CreatePasswordService))
            {
                #region Authorization
                var token = GlobalHelper.AuthorizationToken(context);                               

                if (!String.IsNullOrEmpty(token))
                {
                    BaseResponse response = await UserAuthorization(token: token!);

                    if (!response.Success)
                    {
                        Log.Warning("{Message}-{StatusCode}-{ErrorType}-{ErrorMessage}-{Method}-{BaseUrl}-{Path}-{Host}", response.StatusMessage, response.StatusCode, "API Gateway", response.StatusCode + " : " + response.StatusMessage, context.Request.Method, context.Request.Host.Host, context.Request.Path.ToString(), context.Request.Host.ToString());

                        await MiddlewareErrorHandle(context, new BaseResponse()
                        {
                            Success = response.Success,
                            StatusCode = response.StatusCode,
                            StatusMessage = response.StatusMessage,
                        });
                        return new BaseResponse() { };
                    }

                    await next(context);
                    return new BaseResponse() { };
                }
                #endregion
                else
                {
                    var statusCode = context.Response.StatusCode;
                    var message = CommonMessage.GatewayFailMessage;
                    Log.Warning("{Message}-{StatusCode}-{ErrorType}-{ErrorMessage}-{Method}-{BaseUrl}-{Path}-{Host}", message, statusCode, "API Gateway", statusCode + " : " + message, context.Request.Method, context.Request.Host.Host, context.Request.Path.ToString(), context.Request.Host.ToString());

                    await MiddlewareErrorHandle(context, new BaseResponse()
                    {
                        Success = false,
                        StatusCode = 401,
                        StatusMessage = CommonMessage.UnauthorizedMessage,
                    });
                    return new BaseResponse() { };
                }
            }

            await next(context);
            return new BaseResponse() { };
        }
        #endregion

        #region UserAuthorization
        private async Task<BaseResponse> UserAuthorization(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                Aes256Helper aes256Helper = new(configuration);
                string issuer = aes256Helper.Aes256Decryption(configuration.GetSection("JWTSettings").GetSection("Issuer").Value!);
                string audience = aes256Helper.Aes256Decryption(configuration.GetSection("JWTSettings").GetSection("Audience").Value!);
                string key = aes256Helper.Aes256Decryption(configuration.GetSection("JWTSettings").GetSection("Key").Value!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key!)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                #region Set properties
                var userType = jwtToken.Claims.First(x => x.Type == "UserType").Value;

                if (aes256Helper.Aes256Decryption(userType) is not (ServiceConstant.SuperAdmin or ServiceConstant.Teacher or ServiceConstant.Student))
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        StatusCode = 401,
                        StatusMessage = CommonMessage.InvalidToken,
                    };
                }
                #endregion

                await Task.CompletedTask;
                return new BaseResponse()
                {
                    Success = true,
                    StatusCode = 200,
                };
            }
            catch (SecurityTokenValidationException ex)
            {
                bool isExpired = ex.Message.Contains("Lifetime validation failed");
                string message = isExpired ? CommonMessage.ExpiredToken : CommonMessage.InvalidToken;

                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = 401,
                    StatusMessage = message,
                };
            }
            catch (Exception)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = 401,
                    StatusMessage = CommonMessage.InvalidToken,
                };
            }
        }
        #endregion

        #region MiddlewareErrorHandle
        private static Task MiddlewareErrorHandle(HttpContext context, BaseResponse response)
        {
            HttpStatusCode code = (HttpStatusCode)response.StatusCode;
            string result = JsonConvert.SerializeObject(new
            {
                response.Success,
                response.StatusCode,
                response.StatusMessage
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
        #endregion
    }
}
