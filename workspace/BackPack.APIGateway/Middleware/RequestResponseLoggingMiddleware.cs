using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace BackPack.APIGateway.Middleware
{
    public class RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        public async Task<BaseResponse> InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            await next(context);
            var serviceNameList = context.Request.Path.Value!.Split("/");
            int index = serviceNameList.Length - 1;
            var serviceName = serviceNameList[index];
            if (serviceName != "")
            {
                var statusCode = FormatResponse(context.Response);
                if (!(statusCode == 200 || statusCode == 201))
                {
                    await MiddlewareErrorHandle(context, new BaseResponse()
                    {
                        Success = false,
                        StatusCode = statusCode,
                        StatusMessage = CommonMessage.GatewayFailMessage,
                    });
                    var message = CommonMessage.GatewayFailMessage;
                    Log.Warning("{Message}-{StatusCode}-{ErrorType}-{ErrorMessage}-{Method}-{BaseUrl}-{Path}-{Host}", message, statusCode, "API Gateway", statusCode + " : " + message, context.Request.Method, context.Request.Host.Host, context.Request.Path.ToString(), context.Request.Host.ToString());
                    return new BaseResponse() { };
                }
            }            
            return new BaseResponse() { };
        }        

        #region FormatResponse
        private static int FormatResponse(HttpResponse response)
        {
            int statusCode = response.StatusCode;

            return statusCode;
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

            if (context.Response.ContentType == null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }
            else
            {
                return Task.FromResult(result);
            }
        }
        #endregion
    }
}
