using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IUnlockOfflineLessonPodRepository
    {
        Task<BaseResponse> UnlockOfflineLessonUnitByCourseIDAsync(UnlockOfflineLessonPodRequest request);

        Task<BaseResponse> UnlockOfflineLessonUnitByDistIDAsync(UnlockOfflineLessonPodDistRequest request);

        Task<BaseResponse> UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(UnlockOfflineLessonPodDistRequest request);
    }
}
