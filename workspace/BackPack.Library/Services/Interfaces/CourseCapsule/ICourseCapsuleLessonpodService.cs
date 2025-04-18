using BackPack.Library.Responses.CourseCapsule;

namespace BackPack.Library.Services.Interfaces.CourseCapsule
{
    public interface ICourseCapsuleLessonpodService
    {
        Task<AllLessonPodsBySubjectResponse> AllLessonPodsBySubjectAsync(int loginId, int subjectId);
    }
}
