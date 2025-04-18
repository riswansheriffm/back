using BackPack.Library.Responses.District;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Repositories.Interfaces.District
{   
    public interface IDistrictRepository
    {
        Task<GetDomainAcceptedEvent> GetDistrictAsync(int DomainID);

        Task<GetAllDistrictResponse> GetAllDistrictAsync();

        Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveDomainsAsync();

        Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID);

        Task<GetAllActiveDomainsResponse> GetAllActiveDomainsAsync();
    }
}
