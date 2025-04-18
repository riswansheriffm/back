using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class SyncLessonPodService(
        ISyncLessonPodRepository syncLessonPodRepository
        ) : ISyncLessonPodService
    {
        #region SyncCourseDownloadAsync
        public async Task<SyncCourseDownloadResponse> SyncCourseDownloadAsync(int StudentID)
        {
            SyncCourseDownloadResponse response = await syncLessonPodRepository.SyncCourseDownloadAsync(StudentID);

            return response;
        }
        #endregion

        #region SyncLessonUnitDownloadAsync
        public async Task<SyncLessonUnitDownloadResponse> SyncLessonUnitDownloadAsync(int StudentID, int LessonUnitDistID, int PreviousLessonUnitDistID)
        {
            SyncLessonUnitDownloadResponse response = await syncLessonPodRepository.SyncLessonUnitDownloadAsync(StudentID, LessonUnitDistID, PreviousLessonUnitDistID);

            return response;
        }
        #endregion

        #region SyncCourseLessonUnitDownloadAsync
        public async Task<SyncCourseLessonUnitDownloadResponse> SyncCourseLessonUnitDownloadAsync(int StudentID, string CourseIDs)
        {
            SyncCourseLessonUnitDownloadResponse response = await syncLessonPodRepository.SyncCourseLessonUnitDownloadAsync(StudentID, CourseIDs);

            return response;
        }
        #endregion
    }
}
