using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Helpers.Emails;
using BackPack.Library.Messages;
using BackPack.Library.Repositories;
using BackPack.Library.Repositories.Interfaces.Global;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using BackPack.Library.Services.Interfaces.User;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Services.Services.User
{
    public class UserUploadService(
        IGlobalRepository globalRepository, 
        IUserUploadRepository userUploadRepository,
        ICreateUserRepository createUserRepository, 
        IUpdateUserRepository updateUserRepository,
        IDeleteUserRepository deleteUserRepository, 
        IUpdateStudentRepository updateStudentRepository,
        IConfiguration configuration
        ) : GenericRepository, IUserUploadService
    {
        #region CreateUserUploadAsync
        public async Task<UserUploadResponse> CreateUserUploadAsync(UserUploadRequest request)
        {
            List<UserUploadEmailRequest> listUserEmail = [];
            List<UserListUploadResponse> listUserResponse = [];
            UserUploadResponse response = new();

            #region Check Domain
            var domainCount = await globalRepository.CheckDomainByID(request.DistrictID);

            if (domainCount == 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.DomainNotFound;
                return response;
            }
            #endregion

            #region Check School
            var schoolCount = await globalRepository.CheckSchoolByID(request.SchoolID);

            if (schoolCount == 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.SchoolNotFound;
                return response;
            }
            #endregion

            #region CheckDomainAndSchoolByID
            var domSchoolCount = await globalRepository.CheckDomainAndSchoolByID(request.DistrictID, request.SchoolID);

            if (domSchoolCount == 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.InvalidDomainSchool;
                return response;
            }
            #endregion

            #region Create user
            for (int index = 0; index < request.UserList?.Count; index++)
            {
                var userPasswordSalt = Hash.GetSecureSalt();
                var userPasswordHash = Hash.HashUsingPbkdf2("1234", userPasswordSalt);
                UserListUploadRequest user = request.UserList[index];
                UserUploadQueryRequest userUploadQueryRequest = new()
                {
                    DistrictID = request.DistrictID,
                    SchoolID = request.SchoolID,
                    ActivityBy = request.ActivityBy,
                    LoginName = user.LoginName!,
                    FName = user.FName!,
                    LName = user.LName!,
                    EmailID = user.EmailID!,
                    UserType = user.UserType!,
                    ClassName = user.ClassName!,
                    GmailID = user.GmailID!,
                    PhoneNo = user.PhoneNo!,
                    Password = userPasswordHash,
                    PasswordSalt = Convert.ToBase64String(userPasswordSalt).ToString()
                };

                UserUploadQueryResponse result = await userUploadRepository.CreateUserUploadAsync(userUploadQueryRequest);

                #region Valid teacher list                    
                if (result.ReturnStatus == 1 && user.UserType?.ToLower().ToString() == "teacher")
                {
                    int userID = result.UserID;
                    string domainName = result.DomainName;

                    UserUploadEmailRequest emailRequest = new()
                    {
                        SecurityCode = userID * 100 + 110,
                        LoginName = user.LoginName,
                        FName = user.FName,
                        LName = user.LName,
                        EmailID = user.EmailID,
                        HostName = request.HostName,
                        DomainName = domainName
                    };
                    listUserEmail.Add(emailRequest);
                }
                #endregion

                #region Response user message list                    
                if (result.ReturnStatus == 0)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.UnknownError
                    };
                    listUserResponse.Add(userResponse);
                }

                if (result.ReturnStatus == 2)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.DuplicateTeacher
                    };
                    listUserResponse.Add(userResponse);
                }

                if (result.ReturnStatus == 3)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.DuplicateStudent
                    };
                    listUserResponse.Add(userResponse);
                }

                if (result.ReturnStatus == 4)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.InvalidClass
                    };
                    listUserResponse.Add(userResponse);
                }

                if (result.ReturnStatus == 5)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.TeacherAlreadyMapped
                    };
                    listUserResponse.Add(userResponse);
                }

                if (result.ReturnStatus == 6)
                {
                    UserListUploadResponse userResponse = new()
                    {
                        RowNumber = index + 1,
                        Message = UserMessage.StudentAlreadyMapped
                    };
                    listUserResponse.Add(userResponse);
                }
                #endregion
            }
            #endregion

            #region Send email for teachers                
            if (listUserEmail.Count > 0)
            {
                UserUploadEmail userUploadEmail = new(configuration);
                await userUploadEmail.UserUploadEmailAsync(listUserEmail);
            }
            #endregion

            #region Response
            response.MessageID = CommonMessage.SuccessID;
            response.Success = true;
            response.StatusMessage = UserMessage.UserUploadSuccess;
            response.StatusCode = StatusCodes.Status201Created;
            response.Messages = listUserResponse;
            #endregion

            return response;
        }
        #endregion

        #region CreateUserAsync
        public async Task<BaseResponse> CreateUserAsync(UserRequest request)
        {
            BaseResponse response = await createUserRepository.CreateUserAsync(request);

            #region Email list
            List<UserUploadEmailRequest> listUserEmail = [];
            if (response.Success && request.UserType?.ToLower().ToString() != "student")
            {
                int userID = response.ResultCount;

                UserUploadEmailRequest emailRequest = new()
                {
                    SecurityCode = userID * 100 + 110,
                    LoginName = request.LoginName,
                    FName = request.FName,
                    LName = request.LName,
                    EmailID = request.EmailID,
                    //HostName = request.HostName,
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

        #region UpdateUserAsync
        public async Task<BaseResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            BaseResponse response = await updateUserRepository.UpdateUserAsync(request);

            return response;
        }
        #endregion

        #region DeleteUserAsync
        public async Task<BaseResponse> DeleteUserAsync(DeleteUserRequest request)
        {
            BaseResponse response = await deleteUserRepository.DeleteUserAsync(request);

            return response;
        }
        #endregion

        #region UpdateStudentAsync
        public async Task<BaseResponse> UpdateStudentAsync(UpdateStudentRequest request)
        {
            BaseResponse response = await updateStudentRepository.UpdateStudentAsync(request);

            return response;
        }
        #endregion              
        
    }
}
