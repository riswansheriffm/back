using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.School;
using BackPack.Library.Services.Interfaces.School;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BackPack.Library.Helpers.Emails;
using BackPack.Dependency.Library.Messages;

namespace BackPack.Library.Services.Services.School
{
    public class SchoolService(
        ISchoolRepository schoolRepository,
        ICreateSchoolRepository createSchoolRepository,
        IConfiguration configuration,
        IUpdateSchoolRepository updateSchoolRepository,
        IDeleteSchoolRepository deleteSchoolRepository,
        ICreateSchoolBulkRepository createSchoolBulkRepository
        ) : ISchoolService
    {
        #region GetSchoolAsync 
        public async Task<SchoolResponse> GetSchoolAsync(int SchoolID)
        {
            SchoolResponse response = await schoolRepository.GetSchoolAsync(SchoolID);

            return response; 
        }
        #endregion

        #region GetAllSchoolAsync 
        public async Task<AllSchoolResponse> GetAllSchoolAsync(int DomainID)
        {
            AllSchoolResponse response = await schoolRepository.GetAllSchoolAsync(DomainID);

            return response;
        }
        #endregion

        #region CreateSchoolAsync
        public async Task<CreateSchoolResponse> CreateSchoolAsync(CreateSchoolRequest request)
        {
            if (request.DistrictID != GlobalApplicationProperty.DomainID)
            {
                return new CreateSchoolResponse()
                {
                    MessageID = CommonMessage.NotFoundID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionType = CommonMessage.ExceptionTypeNormal,
                    StatusMessage = CommonMessage.InvalidToken
                };
            }

            CreateSchoolRequest createSchoolRequest = new() 
            { 
                Name = request.Name,
                Desc = request.Desc,
                DistrictID = request.DistrictID,
                ActivityDesc = request.ActivityDesc,
                ActivityBy = request.ActivityBy,
                EmailID = request.EmailID,
                FirstName = request.FirstName, 
                LastName = request.LastName,
                LoginName = request.LoginName,
                PhoneNo = request.PhoneNo,
            };
            CreateSchoolResponse result = await createSchoolRepository.CreateSchoolAsync(createSchoolRequest);

            if (result.ReturnStatus == 0)
            {

                int UserID = result.UserID;
                string DomainName = result.DomainName!;
                int num = UserID * 100 + 110;
                string Scode = num.ToString();

                DistrictUserEmail DistrictUserEmail = new(configuration);
                await DistrictUserEmail.DistrictUserEmailAsync(request.LoginName, request.FirstName, request.LastName, request.EmailID, DomainName, Scode);

                result.MessageID = CommonMessage.SuccessID;
                result.Success = true;
                result.StatusCode = StatusCodes.Status201Created;
                result.StatusMessage = SchoolMessage.SchoolCreated;
            }
            else
            {
                if (result.ReturnStatus > 0)
                {
                    result.MessageID = CommonMessage.DuplicateID;
                    result.Success = false;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.StatusMessage = SchoolMessage.DuplicateUser;
                }
                else
                {
                    result.MessageID = CommonMessage.FailID;
                    result.Success = false;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.StatusMessage = CommonMessage.BadRequestMessage;
                }                
            }
            return result; 
        }
        #endregion

        #region UpdateSchoolAsync
        public async Task<BaseResponse> UpdateSchoolAsync(UpdateSchoolRequest request)
        {
            BaseResponse response = await updateSchoolRepository.UpdateSchoolAsync(request);

            return response;
        }
        #endregion

        #region DeleteSchoolAsync
        public async Task<BaseResponse> DeleteSchoolAsync(DeleteSchoolRequest request)
        {
            BaseResponse response = await deleteSchoolRepository.DeleteSchoolAsync(request);

            return response;
        }
        #endregion

        #region CreateSchoolBulkAsync
        public async Task<BaseResponse> CreateSchoolBulkAsync(CreateSchoolBulkRequest request)
        {
            if (request.DistrictID != GlobalApplicationProperty.DomainID)
            {
                return new BaseResponse()
                {
                    MessageID = CommonMessage.NotFoundID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionType = CommonMessage.ExceptionTypeNormal,
                    StatusMessage = CommonMessage.InvalidToken
                };
            }
            BaseResponse response = await createSchoolBulkRepository.CreateSchoolBulkAsync(request);

            return response;
        }
        #endregion
    }
} 
