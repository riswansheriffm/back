using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IAllActivitiesByLessonPodRepository
    {
        Task<AllActivitiesByLessonPodResponse> AllActivitiesByLessonPodAsync(int LessonUnitID, int LoginID);
    }
}
