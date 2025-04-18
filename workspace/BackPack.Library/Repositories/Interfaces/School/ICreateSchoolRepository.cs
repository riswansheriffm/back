using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.School;

namespace BackPack.Library.Repositories.Interfaces.School
{
    public interface ICreateSchoolRepository
    {
        Task<CreateSchoolResponse> CreateSchoolAsync(CreateSchoolRequest request);
    }
}
