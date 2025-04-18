using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IPendingLessonPodRepository
    {
        Task<PendingLessonPodsForAStudentResponse> PendingLessonPodsForAStudentAsync(int StudentID, int CourseID, int ParentID, int LessonID);
    }
}
