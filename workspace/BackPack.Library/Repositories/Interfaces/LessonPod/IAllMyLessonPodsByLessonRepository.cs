using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IAllMyLessonPodsByLessonRepository
    {
        Task<AllMyLessonPodsByLessonResponse> AllMyLessonPodsByLessonAsync(int LessonID, int LoginID);
    }
}
