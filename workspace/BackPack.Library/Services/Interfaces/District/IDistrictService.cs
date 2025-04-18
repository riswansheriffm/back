using BackPack.Library.Requests.District;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.District;
using BackPack.Library.Responses.School;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Interfaces.District
{
    public interface IDistrictService
    {
        Task<GetDomainAcceptedEvent> GetDistrictAsync(int DomainID);

        Task<GetAllDistrictResponse> GetAllDistrictAsync();

        Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveDomainsAsync();

        Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID);

        Task<GetAllActiveDomainsResponse> GetAllActiveDomainsAsync();

        Task<BaseResponse> UpdateDistrictAsync(UpdateDistrictRequest request);

        Task<BaseResponse> DistrictStatusAsync(DistrictStatusRequest request);

        Task<CreateSchoolResponse> CreateDistrictAsync(CreateDistrictRequest request);
    }
}
