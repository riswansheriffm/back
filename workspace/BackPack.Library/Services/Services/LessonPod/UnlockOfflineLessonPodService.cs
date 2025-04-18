using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class UnlockOfflineLessonPodService(
        IUnlockOfflineLessonPodRepository unlockOfflineLessonPodRepository
        ) : IUnlockOfflineLessonPodService
    {
        #region UnlockOfflineLessonUnitByCourseIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByCourseIDAsync(UnlockOfflineLessonPodRequest request)
        {
            BaseResponse response = await unlockOfflineLessonPodRepository.UnlockOfflineLessonUnitByCourseIDAsync(request);

            return response;
        }
        #endregion

        #region UnlockOfflineLessonUnitByDistIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByDistIDAsync(UnlockOfflineLessonPodDistRequest request)
        {
            BaseResponse response = await unlockOfflineLessonPodRepository.UnlockOfflineLessonUnitByDistIDAsync(request);

            return response;
        }
        #endregion

        #region UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(UnlockOfflineLessonPodDistRequest request)
        {
            BaseResponse response = await unlockOfflineLessonPodRepository.UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(request);

            return response;
        }
        #endregion
    }
}
