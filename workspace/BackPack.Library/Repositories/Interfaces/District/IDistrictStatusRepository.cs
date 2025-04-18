using BackPack.Library.Requests.District;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.District
{
    public interface IDistrictStatusRepository
    {
        Task<BaseResponse> DistrictStatusAsync(DistrictStatusRequest request);
    }
}
