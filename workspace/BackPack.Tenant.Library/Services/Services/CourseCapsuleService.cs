
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using MassTransit;

namespace BackPack.Tenant.Library.Services.Services
{
    public class CourseCapsuleService(
        IGetTenantByDomainIdRepository getTenantByDomainIdRepository,
        IRequestClient<GetCourseCapsuleEvent> requestPublicCourseCapsuleByDomainClient,
        IRequestClient<GetAllSubjectsByDomainEvent> requestAllSubjectsByDomainClient,
        IRequestClient<GetCourseCapsuleByCapsuleEvent> requestCourseCapsuleByCapsuleClient,
        IRequestClient<GetAllCoursesByDomainEvent> requestAllCoursesByDomainClient,
        IRequestClient<GetAllTeacherByClassEvent> requestAllTeacherByClassClient,
        IRequestClient<GetAllLPCourseLicensesEvent> requestAllLPCourseLicensesClient
        ) : ICourseCapsuleService
    {
        #region GetPublicCourseCapsuleByDomainAsync
        public async Task<PublicCourseCapsuleByDomainResponseEvent> GetPublicCourseCapsuleByDomainAsync(int loginId, int domainId, int subjectId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<PublicCourseCapsuleByDomainResponseEvent> consumerResponse = await requestPublicCourseCapsuleByDomainClient!.GetResponse<PublicCourseCapsuleByDomainResponseEvent>(new GetCourseCapsuleEvent()
            {
                DomainId = domainId,
                LoginId = loginId,
                SubjectId = subjectId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new PublicCourseCapsuleByDomainResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion

        #region GetAllSubjectsByDomainAsync
        public async Task<AllSubjectsByDomainResponseEvent> GetAllSubjectsByDomainAsync(int loginId, int domainId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<AllSubjectsByDomainResponseEvent> consumerResponse = await requestAllSubjectsByDomainClient!.GetResponse<AllSubjectsByDomainResponseEvent>(new GetAllSubjectsByDomainEvent()
            {
                DomainId = domainId,
                LoginId = loginId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new AllSubjectsByDomainResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion

        #region GetCourseCapsuleByCapsuleAsync
        public async Task<CourseCapsuleByCapsuleResponseEvent> GetCourseCapsuleByCapsuleAsync(int courseCapsuleId, int domainId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<CourseCapsuleByCapsuleResponseEvent> consumerResponse = await requestCourseCapsuleByCapsuleClient!.GetResponse<CourseCapsuleByCapsuleResponseEvent>(new GetCourseCapsuleByCapsuleEvent()
            {
                DomainId = domainId,
                CourseCapsuleId = courseCapsuleId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new CourseCapsuleByCapsuleResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion


        #region GetAllCoursesByDomainAsync
        public async Task<AllCoursesByDomainResponseEvent> GetAllCoursesByDomainAsync(int domainId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<AllCoursesByDomainResponseEvent> consumerResponse = await requestAllCoursesByDomainClient!.GetResponse<AllCoursesByDomainResponseEvent>(new GetAllCoursesByDomainEvent()
            {
                DomainId = domainId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new AllCoursesByDomainResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion

        #region GetAllTeachersByClassAsync
        public async Task<AllTeacherByClassResponseEvent> GetAllTeachersByClassAsync(int domainId, int courseId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<AllTeacherByClassResponseEvent> consumerResponse = await requestAllTeacherByClassClient!.GetResponse<AllTeacherByClassResponseEvent>(new GetAllTeacherByClassEvent()
            {
                DomainId = domainId,
                CourseID = courseId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new AllTeacherByClassResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion

        #region GetAllLPCourseLicensesAsync
        public async Task<LPCourseLicensesResponseEvent> GetAllLPCourseLicensesAsync(int domainId, int courseId, int courseCapsuleId)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: domainId);

            Response<LPCourseLicensesResponseEvent> consumerResponse = await requestAllLPCourseLicensesClient!.GetResponse<LPCourseLicensesResponseEvent>(new GetAllLPCourseLicensesEvent()
            {
                DomainId = domainId,
                CourseID = courseId,
                CourseCapsuleId = courseCapsuleId,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new LPCourseLicensesResponseEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion
    }
}
