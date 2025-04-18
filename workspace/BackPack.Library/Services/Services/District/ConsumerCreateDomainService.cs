using BackPack.Dependency.Library.Messages;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.District;
using BackPack.Library.Requests.District;
using BackPack.Library.Responses.School;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Services.Services.District
{
    public class OldConsumerCreateDomainService
    {
        private readonly IConfiguration _configuration;
        private readonly ICreateDistrictRepository _createDistrictRepository;
        public OldConsumerCreateDomainService(IConfiguration configuration, ICreateDistrictRepository createDistrictRepository)
        {
            _configuration = configuration;
            _createDistrictRepository = createDistrictRepository;
        }
        #region CreateDistrictAsync
        public async Task<CreateSchoolResponse> CreateDistrictAsync(CreateDistrictRequest request)
        {
            CreateDistrictRequest createDistrictRequest = new()
            {
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
                SourceID = request.SourceID
            };

            CreateSchoolResponse result = await _createDistrictRepository.CreateDistrictAsync(createDistrictRequest);

            if (result.ReturnStatus == 0)
            {
                int UserID = result.UserID;
                string DomainName = result.DomainName!;

                //int num = UserID * 100 + 110;
                //string Scode = num.ToString();
                //DistrictUserEmail DistrictUserEmail = new(_configuration);
                //await DistrictUserEmail.DistrictUserEmailAsync(request.LoginName, request.FirstName, request.LastName, request.EmailID, DomainName, Scode);

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
