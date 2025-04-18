
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class LessonpodStudentGetService(
        IGetLessonpodsForAStudentRepository getLessonpodsForAStudentRepository
        ) : ILessonpodStudentGetService
    {
        #region GetLessonpodsForAStudentAsync
        public async Task<LessonpodsForAStudentResponse> GetLessonpodsForAStudentAsync(int studentId, int courseId, int lessonId, int chapterId, int parentId)
        {
            LessonpodsForAStudentResponse response = await getLessonpodsForAStudentRepository.GetLessonpodsForAStudentAsync(studentId: studentId, courseId: courseId, lessonId: lessonId, chapterId: chapterId, parentId: parentId);

            return response;
        }
        #endregion
    }
}
