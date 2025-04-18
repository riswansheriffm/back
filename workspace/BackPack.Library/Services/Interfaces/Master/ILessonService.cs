using BackPack.Library.Responses.Master.Lesson;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface ILessonService
    {
        Task<AllLessonsForASubjectResponse> AllLessonsForASubjectAsync(int SubjectID, int ChapterID);
    }
}
