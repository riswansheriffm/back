using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Interfaces.LessonPod
{
    public interface IChapterBySubjectRepository
    {
        Task<AllChaptersBySubjectResponse> AllChaptersBySubjectAsync(int SubjectID);
    }
}
