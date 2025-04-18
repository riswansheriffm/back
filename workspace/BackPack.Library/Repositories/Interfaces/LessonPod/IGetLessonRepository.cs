using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IGetLessonRepository
    {
        Task<LessonResponse> GetLessonAsync(int LessonID);
    }
}
