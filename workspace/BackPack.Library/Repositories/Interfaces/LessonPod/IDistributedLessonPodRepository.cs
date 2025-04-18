using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IDistributedLessonPodRepository
    {
        Task<DistributedLessonPodResponse> DistributedLessonPodAsync(int LessonUnitDistID);
    }
}
