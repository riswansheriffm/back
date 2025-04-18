using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICreateLessonRepository
    {
        Task<BaseResponse> CreateLessonAsync(CreateLessonRequest request);
    }
}
