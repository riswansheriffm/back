using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.School
{
    public interface IDeleteSchoolRepository
    {
        Task<BaseResponse> DeleteSchoolAsync(DeleteSchoolRequest request);
    }
}
