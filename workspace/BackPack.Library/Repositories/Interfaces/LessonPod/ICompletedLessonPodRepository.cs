using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ICompletedLessonPodRepository
    {
        Task<CompletedLessonPodsByLessonResponse> CompletedLessonPodsByLessonAsync(int StudentID, int LessonID, int ParentID, int ChapterID);
    }
}
