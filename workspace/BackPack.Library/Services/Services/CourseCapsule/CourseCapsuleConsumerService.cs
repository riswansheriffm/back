
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Services.CourseCapsule
{
    public class CourseCapsuleConsumerService(
        ICourseCapsuleConsumerRepository courseCapsuleConsumerRepository
        ) : ICourseCapsuleConsumerService
    {
        #region GetAllSubjectsByDomainConsumerAsync
        public async Task<AllSubjectsByDomainResponseEvent> GetAllSubjectsByDomainConsumerAsync(int domainId)
        {
            AllSubjectsByDomainResponseEvent response = await courseCapsuleConsumerRepository.GetAllSubjectsByDomainConsumerAsync(domainId: domainId);

            return response;
        }
        #endregion

        #region PublicCourseCapsuleByDomainAndSubjectConsumerAsync
        public async Task<PublicCourseCapsuleByDomainResponseEvent> PublicCourseCapsuleByDomainAndSubjectConsumerAsync(int domainId, int subjectId)
        {
            PublicCourseCapsuleByDomainResponseEvent response = await courseCapsuleConsumerRepository.PublicCourseCapsuleByDomainAndSubjectConsumerAsync(domainId: domainId, subjectId: subjectId);

            return response;
        }
        #endregion

        #region GetCourseCapsuleByCapsuleConsumerAsync
        public async Task<CourseCapsuleByCapsuleResponseEvent> GetCourseCapsuleByCapsuleConsumerAsync(int courseCapsuleId)
        {
            CourseCapsuleByCapsuleResponseEvent response = await courseCapsuleConsumerRepository.GetCourseCapsuleByCapsuleConsumerAsync(courseCapsuleId: courseCapsuleId);

            return response;
        }
        #endregion

        #region GetAllCoursesByDomainConsumerAsync
        public async Task<AllCoursesByDomainResponseEvent> GetAllCoursesByDomainConsumerAsync(int domainId)
        {
            AllCoursesByDomainResponseEvent response = await courseCapsuleConsumerRepository.GetAllCoursesByDomainConsumerAsync(domainId: domainId);

            return response;
        }
        #endregion

        #region GetAllTeacherByClassConsumerAsync
        public async Task<AllTeacherByClassResponseEvent> GetAllTeacherByClassConsumerAsync(int domainId, int courseId)
        {
            AllTeacherByClassResponseEvent response = await courseCapsuleConsumerRepository.GetAllTeacherByClassConsumerAsync(domainId: domainId, courseId: courseId);

            return response;
        }
        #endregion

        #region GetAllLPCourseLicensesConsumerAsync
        public async Task<LPCourseLicensesResponseEvent> GetAllLPCourseLicensesConsumerAsync(int domainId, int courseId, int courseCapsuleId)
        {
            LPCourseLicensesResponseEvent response = await courseCapsuleConsumerRepository.GetAllLPCourseLicensesConsumerAsync(domainId: domainId, courseId: courseId, courseCapsuleId: courseCapsuleId);

            return response;
        }
        #endregion
    }
}
