using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IUpdateLessonRepository 
    {
        Task<BaseResponse> UpdateLessonAsync(UpdateLessonRequest request);
    }
}
