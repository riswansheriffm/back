using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.School
{
    public interface IUpdateSchoolRepository
    {
        Task<BaseResponse> UpdateSchoolAsync(UpdateSchoolRequest request);
    }
}
