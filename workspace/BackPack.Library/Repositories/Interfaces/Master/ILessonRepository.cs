using BackPack.Library.Responses.Master.Lesson;

namespace BackPack.Library.Repositories.Interfaces.Master
{
    public interface ILessonRepository
    {
        Task<AllLessonsForASubjectResponse> AllLessonsForASubjectAsync(int SubjectID, int ChapterID);
    }
}
