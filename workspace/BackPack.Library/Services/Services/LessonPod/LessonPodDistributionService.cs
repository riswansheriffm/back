using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class LessonPodDistributionService(
        ILessonPodDistributionRepository lessonPodDistributionRepository,
        ILessonPodReDistributionRepository lessonPodReDistributionRepository,
        IRecallLessonPodDistributionRepository recallLessonPodDistributionRepository
        ) : ILessonPodDistributionService
    {
        #region LessonPodDistributionAsync
        public async Task<BaseResponse> LessonPodDistributionAsync(LessonPodDistributionRequest request)
        {
            BaseResponse response = await lessonPodDistributionRepository.LessonPodDistributionAsync(request);

            return response;
        }
        #endregion

        #region LessonPodReDistributionAsync
        public async Task<BaseResponse> LessonPodReDistributionAsync(LessonPodReDistributionRequest request)
        {
            BaseResponse response = await lessonPodReDistributionRepository.LessonPodReDistributionAsync(request);

            return response;
        }
        #endregion

        #region RecallLessonPodDistributionAsync
        public async Task<BaseResponse> RecallLessonPodDistributionAsync(RecallLessonPodDistributionRequest request)
        {
            BaseResponse response = await recallLessonPodDistributionRepository.RecallLessonPodDistributionAsync(request);

            return response;
        }
        #endregion
    }
}
