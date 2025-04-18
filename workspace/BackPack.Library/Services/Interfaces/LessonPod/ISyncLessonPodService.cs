using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface ISyncLessonPodService
    {
        Task<SyncCourseDownloadResponse> SyncCourseDownloadAsync(int StudentID);

        Task<SyncLessonUnitDownloadResponse> SyncLessonUnitDownloadAsync(int StudentID, int LessonUnitDistID, int PreviousLessonUnitDistID);

        Task<SyncCourseLessonUnitDownloadResponse> SyncCourseLessonUnitDownloadAsync(int StudentID, string CourseIDs);
    }
}
