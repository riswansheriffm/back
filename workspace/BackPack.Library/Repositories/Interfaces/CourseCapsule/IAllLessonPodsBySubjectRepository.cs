using BackPack.Library.Responses.CourseCapsule;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface IAllLessonPodsBySubjectRepository
    {
        Task<AllLessonPodsBySubjectResponse> AllLessonPodsBySubjectAsync(int loginId, int subjectId);
    }
}
