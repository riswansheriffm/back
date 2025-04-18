using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IAllMyLessonPodsRepository
    {
        Task<AllMyLessonPodsResponse> AllMyLessonPodsAsync(int LessonID, int AuthorID);
    }
}
