
using BackPack.MessageContract.Library;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ICourseCapsuleService
    {
        Task<AllSubjectsByDomainResponseEvent> GetAllSubjectsByDomainAsync(int loginId, int domainId);

        Task<PublicCourseCapsuleByDomainResponseEvent> GetPublicCourseCapsuleByDomainAsync(int loginId, int domainId, int subjectId);

        Task<CourseCapsuleByCapsuleResponseEvent> GetCourseCapsuleByCapsuleAsync(int courseCapsuleId, int domainId);

        Task<AllCoursesByDomainResponseEvent> GetAllCoursesByDomainAsync(int domainId);

        Task<AllTeacherByClassResponseEvent> GetAllTeachersByClassAsync(int domainId, int courseId);

        Task<LPCourseLicensesResponseEvent> GetAllLPCourseLicensesAsync(int domainId, int courseId, int courseCapsuleId);
    }
}
