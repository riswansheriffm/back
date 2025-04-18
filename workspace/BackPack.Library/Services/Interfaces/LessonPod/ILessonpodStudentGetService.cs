
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface ILessonpodStudentGetService
    {
        Task<LessonpodsForAStudentResponse> GetLessonpodsForAStudentAsync(int studentId, int courseId, int lessonId, int chapterId, int parentId);
    }
}
