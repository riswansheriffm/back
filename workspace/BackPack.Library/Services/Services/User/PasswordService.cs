
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Services.Interfaces.User;

namespace BackPack.Library.Services.Services.User
{
    public class PasswordService(IPasswordRepository passwordRepository) : IPasswordService
    {
        #region CreatePasswordAsync
        public async Task<BaseResponse> CreatePasswordAsync(PasswordRequest request)
        {
            BaseResponse response = await passwordRepository.CreatePasswordAsync(request);

            return response;
        }
        #endregion
    }
}
