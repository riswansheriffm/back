
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Responses.Master.Course;
using BackPack.Library.Services.Interfaces.CourseCapsule;

namespace BackPack.Library.Services.Services.CourseCapsule
{
    public class CourseCapsuleGetService(
        ICourseCapsuleRepository courseCapsuleRepository
        ) : ICourseCapsuleGetService
    {
        #region GetAllCourseCapsulesAsync
        public async Task<AllCourseCapsulesResponse> GetAllCourseCapsulesAsync(int LoginID, int SubjectID, string AppType)
        {
            AllCourseCapsulesResponse response = await courseCapsuleRepository.GetAllCourseCapsulesAsync(LoginID, SubjectID, AppType);

            return response;
        }
        #endregion

        #region GetAllCourseCapsulesForASubjectAsync
        public async Task<AllCourseCapsulesForASubjectResponse> GetAllCourseCapsulesForASubjectAsync(int LoginID, int SubjectID, string AppType)
        {
            AllCourseCapsulesForASubjectResponse response = await courseCapsuleRepository.GetAllCourseCapsulesForASubjectAsync(LoginID, SubjectID, AppType);

            return response;
        }
        #endregion

        #region GetAllCourseCapsuleFoldersForACourseCapsuleAsync
        public async Task<FoldersForACourseCapsuleResponse> GetAllCourseCapsuleFoldersForACourseCapsuleAsync(int CourseCapsuleID)
        {
            FoldersForACourseCapsuleResponse response = await courseCapsuleRepository.GetAllCourseCapsuleFoldersForACourseCapsuleAsync(CourseCapsuleID);

            return response;
        }
        #endregion

        #region GetAllPublicCourseCapsuleByDomainAndSubjectAsync
        public async Task<PublicCourseCapsuleByDomainResponse> GetAllPublicCourseCapsuleByDomainAndSubjectAsync(int DomainID, int SubjectID)
        {
            PublicCourseCapsuleByDomainResponse response = await courseCapsuleRepository.GetAllPublicCourseCapsuleByDomainAndSubjectAsync(DomainID, SubjectID);

            return response;
        }
        #endregion

        #region GetAllLPStudentLicensesByCourseCapsuleAsync
        public async Task<LPStudentLicensesByCourseCapsuleResponse> GetAllLPStudentLicensesByCourseCapsuleAsync(int LoginID, int CourseID, int CourseCapsuleID)
        {
            LPStudentLicensesByCourseCapsuleResponse response = await courseCapsuleRepository.GetAllLPStudentLicensesByCourseCapsuleAsync(LoginID, CourseID, CourseCapsuleID);

            return response;
        }
        #endregion

        #region GetAllLPCourseLicensesAsync
        public async Task<LPCourseLicensesResponse> GetAllLPCourseLicensesAsync(int LoginID, int CourseID, int CourseCapsuleID)
        {
            LPCourseLicensesResponse response = await courseCapsuleRepository.GetAllLPCourseLicensesAsync(LoginID, CourseID, CourseCapsuleID);

            return response;
        }
        #endregion

        #region GetCourseCapsuleForRecorderAsync
        public async Task<CourseCapsuleForReorderResponse> GetCourseCapsuleForRecorderAsync(int LoginID, int SubjectID, int CourseCapsuleID)
        {
            CourseCapsuleForReorderResponse response = await courseCapsuleRepository.GetCourseCapsuleForRecorderAsync(LoginID, SubjectID, CourseCapsuleID);

            return response;
        }
        #endregion
    }
}
