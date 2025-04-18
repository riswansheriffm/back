using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.School
{
    public interface ICreateSchoolBulkRepository
    {
        Task<BaseResponse> CreateSchoolBulkAsync(CreateSchoolBulkRequest request);
    }
}
