using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.MessageContract.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Routing;
using BackPack.Library.Services.Interfaces.MassTransitMessage;
using BackPack.Library.Constants;

namespace BackPack.Library.Middleware
{
    public class JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        #region Invoke
        public async Task<BaseResponse> Invoke(HttpContext context, IMassTransitMessageService massTransitMessageService, ITenantByTenantIDService tenantByTenantIDService)
        {
            Aes256Helper aes256Helper = new(configuration);

            #region Error log condition
            if (context.Request.Path.Value!.Contains("favico"))
            {
                await next(context);
                return new BaseResponse() { };
            }
            #endregion

            #region No authorization
            var serviceName = GlobalHelper.ServiceMethodName(context);

            if (serviceName == ServiceConstant.ServiceStatusService)
            {
                context.Request.EnableBuffering();
                using StreamReader reader = new(context.Request.Body, Encoding.UTF8, false, 1024, true);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                await next(context);
                return new BaseResponse() { };
            }
            if (serviceName == ServiceConstant.TeacherLoginService || serviceName == ServiceConstant.StudentLoginService || serviceName == ServiceConstant.RefreshTokenService || serviceName == ServiceConstant.ActivateUserAccountService || serviceName == ServiceConstant.ResetPasswordService || serviceName == ServiceConstant.CreatePasswordService)
            {
                context.Request.EnableBuffering();
                using StreamReader reader = new(context.Request.Body, Encoding.UTF8, false, 1024, true);
                string requestBody = await reader.ReadToEndAsync();

                GetTenantResponseEvent tenant = await TenantResponse(massTransitMessageService, tenantByTenantIDService, requestBody, serviceName);

                if (tenant.TenantName == "")
                {
                    await MiddlewareErrorHandle(context, new BaseResponse()
                    {
                        Success = false,
                        StatusCode = 400,
                        StatusMessage = CommonMessage.BadRequestMessage,
                    });
                    return new BaseResponse() { };
                }
                GlobalApplicationProperty.TenantID = tenant.TenantID;
                GlobalApplicationProperty.TenantName = tenant.TenantName;
                GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(tenant.DBConnection);

                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                #region Authorization
                var token = GlobalHelper.AuthorizationToken(context);

                if (!String.IsNullOrEmpty(token))
                {
                    BaseResponse response = await UserAuthorization(token: token!);

                    if (!response.Success)
                    {
                        await MiddlewareErrorHandle(context, new BaseResponse()
                        {
                            Success = response.Success,
                            StatusCode = response.StatusCode,
                            StatusMessage = response.StatusMessage,
                        });
                        return new BaseResponse() { };
                    }
                }
                #endregion
                else
                {                
                    await MiddlewareErrorHandle(context, new BaseResponse()
                    {
                        Success = false,
                        StatusCode = 401,
                        StatusMessage = CommonMessage.UnauthorizedMessage,
                    });
                    return new BaseResponse() { };
                }                
            }
            #endregion           

            await next(context);
            if (context.Response.StatusCode == 403)
            {
                await MiddlewareErrorHandle(context, new BaseResponse()
                {
                    Success = false,
                    StatusCode = 401,
                    StatusMessage = CommonMessage.UnauthorizedMessage,
                });
                return new BaseResponse() { };
            }
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
                var userID = jwtToken.Claims.First(x => x.Type == "UserID").Value;
                var tenantID = jwtToken.Claims.First(x => x.Type == "TenantID").Value;
                var dbConnection = jwtToken.Claims.First(x => x.Type == "DBConnection").Value;
                var loginName = jwtToken.Claims.First(x => x.Type == "LoginName").Value;
                var userType = jwtToken.Claims.First(x => x.Type == "UserType").Value;
                var tenantName = jwtToken.Claims.First(x => x.Type == "TenantName").Value;
                var domainID = jwtToken.Claims.First(x => x.Type == "DomainID").Value;
                var userRole = jwtToken.Claims.First(x => x.Type == "UserRole").Value;

                if (aes256Helper.Aes256Decryption(userType) is not (ServiceConstant.Teacher or ServiceConstant.Student))
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        StatusCode = 401,
                        StatusMessage = CommonMessage.InvalidToken,
                    };
                }
                var userTypeID = 0;
                switch (aes256Helper.Aes256Decryption(userType))
                {
                    case "Teacher":
                        userTypeID = 1;
                        break;
                    case "Student":
                        userTypeID = 2;
                        break;
                    default:
                        break;
                }

                GlobalApplicationProperty.TenantID = Guid.Parse(aes256Helper.Aes256Decryption(tenantID));
                GlobalApplicationProperty.TenantName = aes256Helper.Aes256Decryption(tenantName);
                GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(dbConnection);
                GlobalApplicationProperty.UserID = int.Parse(aes256Helper.Aes256Decryption(userID));
                GlobalApplicationProperty.LoginName = aes256Helper.Aes256Decryption(loginName);
                GlobalApplicationProperty.UserType = aes256Helper.Aes256Decryption(userType);
                GlobalApplicationProperty.UserTypeID = userTypeID;
                GlobalApplicationProperty.DomainID = int.Parse(aes256Helper.Aes256Decryption(domainID));
                GlobalApplicationProperty.UserRole = aes256Helper.Aes256Decryption(userRole);
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

        #region TenantResponse
        private static async Task<GetTenantResponseEvent> TenantResponse(IMassTransitMessageService massTransitMessageService, ITenantByTenantIDService tenantByTenantIDService, string requestBody, string serviceName)
        {
            GetTenantResponseEvent response = new();

            if (serviceName == ServiceConstant.TeacherLoginService || serviceName == ServiceConstant.StudentLoginService || serviceName == ServiceConstant.ActivateUserAccountService || serviceName == ServiceConstant.ResetPasswordService || serviceName == ServiceConstant.CreatePasswordService)
            {
                string requestValue = DomainNameResponse(requestBody);
                response = await massTransitMessageService.GetTenantEventResponse(tenantName: requestValue);
            }

            if (serviceName == ServiceConstant.RefreshTokenService)
            {
                Guid requestGuid = TenantIdResponse(requestBody);
                response = await tenantByTenantIDService.GetTenantByIDEventResponse(tenantID: requestGuid);
            }

            return response;
        }
        #endregion

        #region TenantResponse
        private static Guid TenantIdResponse(string request)
        {
            Guid response = Guid.Empty;
            JObject requestObject = JObject.Parse(request);
            foreach (var item in requestObject)
            {
                string requestKey = item.Key;
                if (requestKey == "TenantID")
                {
                    response = (Guid)item.Value!;
                    break;
                }
            }

            return response;
        }
        #endregion

        #region DomainNameResponse
        private static string DomainNameResponse(string request)
        {
            string response = "";
            JObject requestObject = JObject.Parse(request);
            foreach (var item in requestObject)
            {
                string requestKey = item.Key;
                if (requestKey == "DistrictName")
                {
                    response = item.Value!.ToString();
                    break;
                }
            }

            return response;
        }
        #endregion        
    }
}
