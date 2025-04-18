using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface IUnlockOfflineLessonPodService
    {
        Task<BaseResponse> UnlockOfflineLessonUnitByCourseIDAsync(UnlockOfflineLessonPodRequest request);

        Task<BaseResponse> UnlockOfflineLessonUnitByDistIDAsync(UnlockOfflineLessonPodDistRequest request);

        Task<BaseResponse> UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(UnlockOfflineLessonPodDistRequest request);
    }
}
