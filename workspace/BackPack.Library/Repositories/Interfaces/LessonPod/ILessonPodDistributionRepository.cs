using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ILessonPodDistributionRepository
    {
        Task<BaseResponse> LessonPodDistributionAsync(LessonPodDistributionRequest request);
    }
}
