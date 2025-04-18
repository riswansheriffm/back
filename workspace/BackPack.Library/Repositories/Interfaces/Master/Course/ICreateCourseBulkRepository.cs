using BackPack.Library.Requests.Master.Course;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Master.Course
{
    public interface ICreateCourseBulkRepository
    {
        Task<BaseResponse> CreateCourseBulkAsync(CreateCourseBulkRequest request);
    }
}
