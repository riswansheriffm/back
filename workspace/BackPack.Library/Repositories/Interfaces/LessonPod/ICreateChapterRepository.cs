using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICreateChapterRepository
    {
        Task<BaseResponse> CreateChapterAsync(CreateChapterRequest request);
    }
}
