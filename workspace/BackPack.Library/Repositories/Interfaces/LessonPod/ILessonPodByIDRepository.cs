using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ILessonPodByIDRepository
    {
        Task<LessonPodResponse> LessonPodAsync(int LessonUnitID, int AuthorID);
    }
}
