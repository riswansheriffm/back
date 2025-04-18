using BackPack.Library.Requests.District;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.District
{
    public interface IUpdateDistrictRepository
    {
        Task<BaseResponse> UpdateDistrictAsync(UpdateDistrictRequest request);
    }
}
