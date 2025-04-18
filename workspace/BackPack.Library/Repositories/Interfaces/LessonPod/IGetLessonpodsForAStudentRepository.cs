
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IGetLessonpodsForAStudentRepository
    {
        Task<LessonpodsForAStudentResponse> GetLessonpodsForAStudentAsync(int studentId, int courseId, int lessonId, int chapterId, int parentId);
    }
}
