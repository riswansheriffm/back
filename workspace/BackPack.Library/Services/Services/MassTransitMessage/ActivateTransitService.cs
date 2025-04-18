
using BackPack.Library.Services.Interfaces.MassTransitMessage;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BackPack.Library.Services.Services.MassTransitMessage
{
    public class ActivateTransitService : IActivateTransitService
    {
        #region ActivateTransitAsync
        public async Task<bool> ActivateTransitAsync(string transitURL)
        {
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri(transitURL)
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
