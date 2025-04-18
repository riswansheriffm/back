
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Services.Interfaces.CourseCapsule;

namespace BackPack.Library.Services.Services.CourseCapsule
{
    public class CourseCapsuleLessonpodService(
        IAllLessonPodsBySubjectRepository allLessonPodsBySubjectRepository
        ) : ICourseCapsuleLessonpodService
    {
        #region AllLessonPodsBySubjectAsync
        public async Task<AllLessonPodsBySubjectResponse> AllLessonPodsBySubjectAsync(int loginId, int subjectId)
        {
            AllLessonPodsBySubjectResponse response = await allLessonPodsBySubjectRepository.AllLessonPodsBySubjectAsync(loginId: loginId, subjectId: subjectId);

            return response;
        }
        #endregion
    }
}
