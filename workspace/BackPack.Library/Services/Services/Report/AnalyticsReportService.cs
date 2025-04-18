using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using BackPack.Library.Services.Interfaces.Report;

namespace BackPack.Library.Services.Services.Report
{
    public class AnalyticsReportService(
        ICourseSummaryReportRepository courseSummaryReportRepository,
        ILessonPodSummaryReportRepository lessonPodSummaryReportRepository,
        ILessonPodActivityReportRepository lessonPodActivityReportRepository,
        IAllChaptersBySubjectReportRepository allChaptersBySubjectReportRepository,
        ICRLessonPodSummaryReportRepository crLessonPodSummaryReportRepository,
        ICRLessonPodActivityReportRepository crLessonPodActivityReportRepository,
        ICRLessonPodActivityDetailReportRepository crLessonPodActivityDetailReportRepository,
        IClassLevelCourseSummaryAnalyticsReportRepository classLevelCourseSummaryAnalyticsReportRepository,
        ICRDistLessonPodSummaryTierReportRepository crDistLessonPodSummaryTierReportRepository,
        ICRDistLessonPodTierStudentActivityReportRepository crDistLessonPodTierStudentActivityReportRepository,
        ICRSubjectSummaryTagReportRepository crSubjectSummaryTagReportRepository,
        ICRLessonFolderSummaryTagReportRepository crLessonFolderSummaryTagReportRepository,
        ICRChapterSummaryTagReportRepository crChapterSummaryTagReportRepository,
        ICRLessonFoldersBySubjectReportRepository crLessonFoldersBySubjectReportRepository,
        ICRDistributedLessonUnitsByLessonReportRepository crDistributedLessonUnitsByLessonReportRepository,
        ICRClassSummaryTagReportRepository crClassSummaryTagReportRepository,
        ICRStudentListReportRepository crStudentListReportRepository,
        ICRStudentWorkReportRepository crStudentWorkReportRepository
        ) : IAnalyticsReportService
    {
        #region CourseSummaryAnalyticsReportAsync
        public async Task<CourseSummaryAnalyticsReportResponse> CourseSummaryAnalyticsReportAsync(int DomainID, int CourseID, int StudentID)
        {
            CourseSummaryAnalyticsReportResponse response = await courseSummaryReportRepository.CourseSummaryAnalyticsReportAsync(DomainID, CourseID, StudentID);

            return response;
        }
        #endregion

        #region LessonPodSummaryAnalyticsReportAsync
        public async Task<LessonPodSummaryAnalyticsReportResponse> LessonPodSummaryAnalyticsReportAsync(int LessonUnitDistID, int StudentID)
        {
            LessonPodSummaryAnalyticsReportResponse response = await lessonPodSummaryReportRepository.LessonPodSummaryAnalyticsReportAsync(LessonUnitDistID, StudentID);

            return response;
        }
        #endregion

        #region LessonPodActivityAnalyticsReportAsync
        public async Task<LessonPodActivityAnalyticsReportResponse> LessonPodActivityAnalyticsReportAsync(int ContentID, int StudentID)
        {
            LessonPodActivityAnalyticsReportResponse response = await lessonPodActivityReportRepository.LessonPodActivityAnalyticsReportAsync(ContentID, StudentID);

            return response;
        }
        #endregion

        #region AllChaptersBySubjectReportAsync
        public async Task<AllChaptersBySubjectReportResponse> AllChaptersBySubjectReportAsync(int SubjectID, int CourseID)
        {
            AllChaptersBySubjectReportResponse response = await allChaptersBySubjectReportRepository.AllChaptersBySubjectReportAsync(SubjectID, CourseID);

            return response;
        }
        #endregion

        #region CRLessonPodSummaryReportAsync
        public async Task<CRLessonPodSummaryReportResponse> CRLessonPodSummaryReportAsync(int LoginID, int LessonUnitDistID)
        {
            CRLessonPodSummaryReportResponse response = await crLessonPodSummaryReportRepository.CRLessonPodSummaryReportAsync(LoginID, LessonUnitDistID);
            
            return response;
        }
        #endregion

        #region CRLessonPodActivityReportAsync
        public async Task<CRLessonPodActivityReportResponse> CRLessonPodActivityReportAsync(int LoginID, int ContentID)
        {
            CRLessonPodActivityReportResponse response = await crLessonPodActivityReportRepository.CRLessonPodActivityReportAsync(LoginID, ContentID);

            return response;
        }
        #endregion

        #region CRLessonPodActivityDetailReportAsync
        public async Task<CRLessonPodActivityDetailReportResponse> CRLessonPodActivityDetailReportAsync(int LoginID, int ContentID)
        {
            CRLessonPodActivityDetailReportResponse response = await crLessonPodActivityDetailReportRepository.CRLessonPodActivityDetailReportAsync(LoginID, ContentID);

            return response;
        }
        #endregion

        #region ClassLevelCourseSummaryAnalyticsReportAsync
        public async Task<ClassLevelCourseSummaryAnalyticsReportResponse> ClassLevelCourseSummaryAnalyticsReportAsync(int DomainID, int CourseID)
        {
            ClassLevelCourseSummaryAnalyticsReportResponse response = await classLevelCourseSummaryAnalyticsReportRepository.ClassLevelCourseSummaryAnalyticsReportAsync(DomainID, CourseID);

            return response;
        }
        #endregion

        #region CRDistLessonPodSummaryTierReportAsync
        public async Task<CRDistLessonPodSummaryTierReportResponse> CRDistLessonPodSummaryTierReportAsync(int LoginID, int LessonUnitDistID)
        {
            CRDistLessonPodSummaryTierReportResponse response = await crDistLessonPodSummaryTierReportRepository.CRDistLessonPodSummaryTierReportAsync(LoginID, LessonUnitDistID);

            return response;
        }
        #endregion

        #region CRDistLessonPodTierStudentActivityReportAsync
        public async Task<CRDistLessonPodTierStudentActivityReportResponse> CRDistLessonPodTierStudentActivityReportAsync(int LoginID, int LessonUnitDistID, string TierName, int MinRange, int MaxRange)
        {
            CRDistLessonPodTierStudentActivityReportResponse response = await crDistLessonPodTierStudentActivityReportRepository.CRDistLessonPodTierStudentActivityReportAsync(LoginID, LessonUnitDistID, TierName, MinRange, MaxRange);

            return response;
        }
        #endregion

        #region CRSubjectSummaryTagReportAsync
        public async Task<CRSubjectSummaryTagReportResponse> CRSubjectSummaryTagReportAsync(int SubjectID, int StudentID)
        {
            CRSubjectSummaryTagReportResponse response = await crSubjectSummaryTagReportRepository.CRSubjectSummaryTagReportAsync(SubjectID, StudentID);

            return response;
        }
        #endregion

        #region CRLessonFolderSummaryTagReportAsync
        public async Task<CRLessonFolderSummaryTagReportResponse> CRLessonFolderSummaryTagReportAsync(int LessonID)
        {
            CRLessonFolderSummaryTagReportResponse response = await crLessonFolderSummaryTagReportRepository.CRLessonFolderSummaryTagReportAsync(LessonID);

            return response;
        }
        #endregion

        #region CRChapterSummaryTagReportAsync
        public async Task<CRChapterSummaryTagReportResponse> CRChapterSummaryTagReportAsync(int ChapterID)
        {
            CRChapterSummaryTagReportResponse response = await crChapterSummaryTagReportRepository.CRChapterSummaryTagReportAsync(ChapterID);

            return response;
        }
        #endregion

        #region CRLessonFoldersBySubjectReportAsync
        public async Task<CRLessonFoldersBySubjectReportResponse> CRLessonFoldersBySubjectReportAsync(int DomainID, int AuthorID, int SubjectID, int CourseID, int ChapterID)
        {
            CRLessonFoldersBySubjectReportResponse response = await crLessonFoldersBySubjectReportRepository.CRLessonFoldersBySubjectReportAsync(DomainID, AuthorID, SubjectID, CourseID, ChapterID);

            return response;
        }
        #endregion

        #region CRDistributedLessonUnitsByLessonReportAsync
        public async Task<CRDistributedLessonUnitsByLessonReportResponse> CRDistributedLessonUnitsByLessonReportAsync(int AuthorID, int LessonID, int CourseID)
        {
            CRDistributedLessonUnitsByLessonReportResponse response = await crDistributedLessonUnitsByLessonReportRepository.CRDistributedLessonUnitsByLessonReportAsync(AuthorID, LessonID, CourseID);

            return response;
        }
        #endregion

        #region CRClassSummaryTagReportAsync
        public async Task<CRClassSummaryTagReportResponse> CRClassSummaryTagReportAsync(int CourseID)
        {
            CRClassSummaryTagReportResponse response = await crClassSummaryTagReportRepository.CRClassSummaryTagReportAsync(CourseID);

            return response;
        }
        #endregion

        #region CRStudentListReportAsync
        public async Task<CRStudentListReportResponse> CRStudentListReportAsync(int LessonUnitDistID, int AuthorID)
        {
            CRStudentListReportResponse response = await crStudentListReportRepository.CRStudentListReportAsync(LessonUnitDistID, AuthorID);

            return response;
        }
        #endregion

        #region CRStudentWorkReportAsync
        public async Task<CRStudentWorkReportResponse> CRStudentWorkReportAsync(int LessonUnitDistID, int StudentID, int AuthorID)
        {
            CRStudentWorkReportResponse response = await crStudentWorkReportRepository.CRStudentWorkReportAsync(LessonUnitDistID, StudentID, AuthorID);

            return response;
        }
        #endregion
    }
}
