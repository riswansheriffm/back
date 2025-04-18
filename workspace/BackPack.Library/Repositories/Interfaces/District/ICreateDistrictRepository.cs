using BackPack.Library.Requests.District;
using BackPack.Library.Responses.School;

namespace BackPack.Library.Repositories.Interfaces.District
{
    public interface ICreateDistrictRepository
    {
        Task<CreateSchoolResponse> CreateDistrictAsync(CreateDistrictRequest request);
    }
}
