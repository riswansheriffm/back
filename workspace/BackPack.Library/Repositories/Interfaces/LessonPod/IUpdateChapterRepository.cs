using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IUpdateChapterRepository 
    {
        Task<BaseResponse> UpdateChapterAsync(UpdateChapterRequest request);
    }
}
