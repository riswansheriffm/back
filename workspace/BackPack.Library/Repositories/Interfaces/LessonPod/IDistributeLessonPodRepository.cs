using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IDistributeLessonPodRepository
    {
        Task<DistributeLessonPodResponse> DistributeLessonPodAsync(int LessonUnitID, string LessonType);
    }
}
