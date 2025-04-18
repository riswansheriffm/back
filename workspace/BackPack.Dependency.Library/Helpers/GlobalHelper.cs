
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BackPack.Dependency.Library.Helpers
{
    public static class GlobalHelper
    {
        #region ServiceMethodName
        public static string ServiceMethodName(HttpContext context)
        {
            var response = "";

            if (!String.IsNullOrEmpty(context.Request.Path.Value))
            {
                var serviceNameList = context.Request.Path.Value!.Split("/");
                int index = serviceNameList.Length - 1;
                response = serviceNameList[index];
            }

            return response;
        }
        #endregion

        #region AuthorizationToken
        public static string AuthorizationToken(HttpContext context)
        {
            var response = "";

            if (!String.IsNullOrEmpty(context.Request.Headers.Authorization))
            {
                var tokenList = context.Request.Headers.Authorization.FirstOrDefault()!.Split(" ");
                int tokenIndex = tokenList.Length - 1;
                response = tokenList[tokenIndex];
            }

            return response;
        }
        #endregion

        #region StringToString
        public static string StringToString(string request)
        {
            string response = string.Empty;
            if (request != "" && request != null)
            {
                response = $"FETCH ALL IN \"{request}\"";
            }

            return response;
        }
        #endregion

        #region ActivateTransitAsync
        public static async Task<bool> ActivateTransitAsync(string request)
        {
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri(request)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage serverResponse = await client.GetAsync("").ConfigureAwait(false);

                return ((int)serverResponse.StatusCode == StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
