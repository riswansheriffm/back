using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface ILessonPodDetailsRepository
    {
        Task<LessonPodDetailsForAStudentResponse> LessonPodDetailsForAStudentAsync(int StudentID, int LessonUnitDistID, int ParentID);
    }
}
