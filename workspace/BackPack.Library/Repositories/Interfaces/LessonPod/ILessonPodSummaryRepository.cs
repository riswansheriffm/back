using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ILessonPodSummaryRepository
    {
        Task<LessonPodSummaryForAStudentResponse> LessonPodSummaryForAStudentAsync(int StudentID, int LessonUnitDistID);
    }
}
