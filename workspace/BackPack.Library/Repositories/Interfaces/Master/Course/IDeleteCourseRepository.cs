using BackPack.Library.Requests.Master.Course;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.Master.Course
{
    public interface IDeleteCourseRepository
    {
        Task<BaseResponse> DeleteCourseAsync(DeleteCourseRequest request);
    }
}
