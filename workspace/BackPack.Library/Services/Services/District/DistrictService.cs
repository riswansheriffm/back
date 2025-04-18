using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.District;
using BackPack.Library.Requests.District;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.District;
using BackPack.Library.Responses.School;
using BackPack.Library.Services.Interfaces.District;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BackPack.Library.Helpers.Emails;
using BackPack.Library.Messages;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Services.District
{
    public class DistrictService(
        IDistrictRepository districtRepository,
        IUpdateDistrictRepository updateDistrictRepository,
        IDistrictStatusRepository districtStatusRepository,
        ICreateDistrictRepository createDistrictRepository,
        IConfiguration configuration
        ) : IDistrictService
    {
        #region GetDistrictAsync
        public async Task<GetDomainAcceptedEvent> GetDistrictAsync(int DomainID)
        {
            GetDomainAcceptedEvent response = await districtRepository.GetDistrictAsync(DomainID);

            return response;
        }
        #endregion

        #region GetAllDistrictAsync
        public async Task<GetAllDistrictResponse> GetAllDistrictAsync()
        {
            GetAllDistrictResponse response = await districtRepository.GetAllDistrictAsync();

            return response;
        }
        #endregion

        #region GetAllPublicActiveDomainsAsync
        public async Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveDomainsAsync()
        {
            AllPublicActiveDomainsResultResponse response = await districtRepository.GetAllPublicActiveDomainsAsync();

            return response;
        }
        #endregion

        #region GetAllSubjectsByDomainAsync
        public async Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID)
        {
            AllSubjectsByDomainResponse response = await districtRepository.GetAllSubjectsByDomainAsync(DomainID);

            return response;
        }
        #endregion

        #region GetAllActiveDomainsAsync
        public async Task<GetAllActiveDomainsResponse> GetAllActiveDomainsAsync()
        {
            GetAllActiveDomainsResponse response = await districtRepository.GetAllActiveDomainsAsync();

            return response;
        }
        #endregion

        #region UpdateDistrictAsync
        public async Task<BaseResponse> UpdateDistrictAsync(UpdateDistrictRequest request)
        {
            BaseResponse response = await updateDistrictRepository.UpdateDistrictAsync(request);

            return response;
        }
        #endregion 

        #region DistrictStatusAsync
        public async Task<BaseResponse> DistrictStatusAsync(DistrictStatusRequest request)
        {
            BaseResponse response = await districtStatusRepository.DistrictStatusAsync(request);

            return response;
        }
        #endregion

        #region CreateDistrictAsync
        public async Task<CreateSchoolResponse> CreateDistrictAsync(CreateDistrictRequest request)
        {
            CreateDistrictRequest createDistrictRequest = new()
            {
                TenantID = request.TenantID,
                Country = request.Country,
                Name = request.Name,
                Desc = request.Desc,
                StreetAddress = request.StreetAddress,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                MaxStudents = request.MaxStudents,
                MaxTeachers = request.MaxTeachers,
                ActivityBy = request.ActivityBy,    
                AccessType = request.AccessType,
                EmailID = request.EmailID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                LoginName = request.LoginName,
                PhoneNo = request.PhoneNo,
                AccessToken = request.AccessToken,
                ApplicationID = request.ApplicationID,
                SourceID = request.SourceID,
            }; 

            CreateSchoolResponse result = await createDistrictRepository.CreateDistrictAsync(createDistrictRequest);

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
                result.StatusMessage = DistrictMessage.DistrcitCreated;
            }
            else
            {
                result.MessageID = CommonMessage.DuplicateID;
                result.Success = false;
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.StatusMessage = DistrictMessage.DuplicateUser;
            }

            return result;
        }
        #endregion
    }
} 
