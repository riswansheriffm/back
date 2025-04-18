
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Interfaces.CourseCapsule
{
    public interface ICourseCapsuleConsumerService
    {
        Task<AllSubjectsByDomainResponseEvent> GetAllSubjectsByDomainConsumerAsync(int domainId);

        Task<PublicCourseCapsuleByDomainResponseEvent> PublicCourseCapsuleByDomainAndSubjectConsumerAsync(int domainId, int subjectId);

        Task<CourseCapsuleByCapsuleResponseEvent> GetCourseCapsuleByCapsuleConsumerAsync(int courseCapsuleId);

        Task<AllCoursesByDomainResponseEvent> GetAllCoursesByDomainConsumerAsync(int domainId);

        Task<AllTeacherByClassResponseEvent> GetAllTeacherByClassConsumerAsync(int domainId, int courseId);

        Task<LPCourseLicensesResponseEvent> GetAllLPCourseLicensesConsumerAsync(int domainId, int courseId, int courseCapsuleId);
    }
}
