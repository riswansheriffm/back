using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRDistLessonPodTierStudentActivityReportRepository
    {
        Task<CRDistLessonPodTierStudentActivityReportResponse> CRDistLessonPodTierStudentActivityReportAsync(int LoginID, int LessonUnitDistID, string TierName, int MinRange, int MaxRange);
    }
}
