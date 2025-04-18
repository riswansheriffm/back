using BackPack.Library.Responses.Report;

namespace BackPack.Library.Services.Interfaces.Report
{
    public interface IAnalyticsReportService
    {
        Task<CourseSummaryAnalyticsReportResponse> CourseSummaryAnalyticsReportAsync(int DomainID, int CourseID, int StudentID);

        Task<LessonPodSummaryAnalyticsReportResponse> LessonPodSummaryAnalyticsReportAsync(int LessonUnitDistID, int StudentID);

        Task<LessonPodActivityAnalyticsReportResponse> LessonPodActivityAnalyticsReportAsync(int ContentID, int StudentID);

        Task<AllChaptersBySubjectReportResponse> AllChaptersBySubjectReportAsync(int SubjectID, int CourseID);

        Task<CRLessonPodSummaryReportResponse> CRLessonPodSummaryReportAsync(int LoginID, int LessonUnitDistID);

        Task<CRLessonPodActivityReportResponse> CRLessonPodActivityReportAsync(int LoginID, int ContentID);

        Task<CRLessonPodActivityDetailReportResponse> CRLessonPodActivityDetailReportAsync(int LoginID, int ContentID);

        Task<ClassLevelCourseSummaryAnalyticsReportResponse> ClassLevelCourseSummaryAnalyticsReportAsync(int DomainID, int CourseID);

        Task<CRDistLessonPodSummaryTierReportResponse> CRDistLessonPodSummaryTierReportAsync(int LoginID, int LessonUnitDistID);

        Task<CRDistLessonPodTierStudentActivityReportResponse> CRDistLessonPodTierStudentActivityReportAsync(int LoginID, int LessonUnitDistID, string TierName, int MinRange, int MaxRange);

        Task<CRSubjectSummaryTagReportResponse> CRSubjectSummaryTagReportAsync(int SubjectID, int StudentID);

        Task<CRLessonFolderSummaryTagReportResponse> CRLessonFolderSummaryTagReportAsync(int LessonID);

        Task<CRChapterSummaryTagReportResponse> CRChapterSummaryTagReportAsync(int ChapterID);

        Task<CRLessonFoldersBySubjectReportResponse> CRLessonFoldersBySubjectReportAsync(int DomainID, int AuthorID, int SubjectID, int CourseID, int ChapterID);

        Task<CRDistributedLessonUnitsByLessonReportResponse> CRDistributedLessonUnitsByLessonReportAsync(int AuthorID, int LessonID, int CourseID);

        Task<CRClassSummaryTagReportResponse> CRClassSummaryTagReportAsync(int CourseID);

        Task<CRStudentListReportResponse> CRStudentListReportAsync(int LessonUnitDistID, int AuthorID);

        Task<CRStudentWorkReportResponse> CRStudentWorkReportAsync(int LessonUnitDistID, int StudentID, int AuthorID);


    }
}
