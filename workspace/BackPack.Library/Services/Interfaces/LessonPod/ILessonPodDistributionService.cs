using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface ILessonPodDistributionService
    {
        Task<BaseResponse> LessonPodDistributionAsync(LessonPodDistributionRequest request);

        Task<BaseResponse> LessonPodReDistributionAsync(LessonPodReDistributionRequest request);

        Task<BaseResponse> RecallLessonPodDistributionAsync(RecallLessonPodDistributionRequest request);
    }
}
