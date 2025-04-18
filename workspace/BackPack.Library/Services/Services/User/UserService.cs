using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using BackPack.Library.Services.Interfaces.User;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;

namespace BackPack.Library.Services.Services.User
{    
    public class UserService(IUserRepository userRepository) : IUserService
    {
        #region ResetPasswordAsync
        public async Task<BaseResponse> ActivateUserAccountAsync(ResetPasswordRequest request)
        {
            var userPasswordSalt = Hash.GetSecureSalt();
            var userPasswordHash = Hash.HashUsingPbkdf2(request.Password!, userPasswordSalt);

            BaseResponse response = await userRepository.ActivateUserAccountAsync(request, userPasswordHash, Convert.ToBase64String(userPasswordSalt).ToString());

            #region Response
            if (!response.Success)
            {
                return response;
            }

            if (response.ResultCount == 1)
            {
                response.MessageID = CommonMessage.SuccessID;
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.StatusMessage = UserMessage.ResetCredentialSucess;
                return response;
            }

            if (response.ResultCount == 2)
            {
                response.MessageID = CommonMessage.DuplicateID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.ActivateUserAccountAlreadyActivated;
                return response;
            }
            else
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                return response;
            }
            #endregion
        }
        #endregion

        #region ResetPasswordAsync
        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var userPasswordSalt = Hash.GetSecureSalt();
            var userPasswordHash = Hash.HashUsingPbkdf2(request.Password!, userPasswordSalt);

            BaseResponse response = await userRepository.ResetPasswordAsync(request, userPasswordHash, Convert.ToBase64String(userPasswordSalt).ToString());

            #region Response
            if (!response.Success)
            {
                return response;
            }

            if (response.ResultCount > 0)
            {
                response.MessageID = CommonMessage.SuccessID;
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.StatusMessage = UserMessage.ResetCredentialSucess;
                return response;
            }
            else
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                return response;
            }
            #endregion
        }
        #endregion

        #region UserAsync
        public async Task<UserResponse> UserAsync(int LoginID)
        {
            UserResponse response = await userRepository.UserAsync(LoginID);

            return response;
        }
        #endregion

        #region AllUsersAsync
        public async Task<AllUserResponse> AllUsersAsync(int DomainID, int SchoolID, string RoleName)
        {
            AllUserResponse response = await userRepository.AllUsersAsync(DomainID, SchoolID, RoleName);

            return response;
        }
        #endregion

        #region AllDomainUserAsync
        public async Task<AllDomainUsersResponse> AllDomainUserAsync(int DomainID, int SchoolID, string Role)
        {
            AllDomainUsersResponse response = await userRepository.AllDomainUserAsync(DomainID, SchoolID, Role);

            return response;
        }
        #endregion

        #region AllTeacherByClassAsync
        public async Task<AllTeacherByClassResponse> AllTeacherByClassAsync(int LoginID, int CourseID)
        {
            AllTeacherByClassResponse response = await userRepository.AllTeacherByClassAsync(LoginID, CourseID);

            return response;
        }
        #endregion

        #region SuperCaAsync
        public async Task<SuperCaResponse> SuperCaAsync(int domainId)
        {
            SuperCaResponse response = await userRepository.SuperCaAsync(domainId);

            return response;
        }
        #endregion
    }
}
