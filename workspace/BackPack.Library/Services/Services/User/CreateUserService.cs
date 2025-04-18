
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Helpers.Emails;
using BackPack.Library.Repositories;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Services.Interfaces.User;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Services.Services.User
{
    public class CreateUserService(
        ICreateSuperUserRepository createSuperUserRepository,
        IConfiguration configuration
        ) : GenericRepository, ICreateUserService
    {
        #region CreateSuperUserAsync
        public async Task<BaseResponse> CreateSuperUserAsync(SuperUserRequest request)
        {
            BaseResponse response = await createSuperUserRepository.CreateSuperUserAsync(request);

            #region Email list
            List<UserUploadEmailRequest> listUserEmail = [];
            if (response.Success)
            {
                int userID = response.ResultCount;

                UserUploadEmailRequest emailRequest = new()
                {
                    SecurityCode = userID * 100 + 110,
                    LoginName = request.LoginName,
                    FName = request.FName,
                    LName = request.LName,
                    EmailID = request.EmailID,
                    DomainName = response.DomainName
                };
                listUserEmail.Add(emailRequest);
            }
            #endregion

            #region Send email                
            if (listUserEmail.Count > 0)
            {
                UserUploadEmail userUploadEmail = new(configuration);
                await userUploadEmail.UserUploadEmailAsync(listUserEmail);
            }
            #endregion

            return response;
        }
        #endregion
    }
}
