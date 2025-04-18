
using BackPack.Library.Responses.Master.Course;

namespace BackPack.Library.Services.Interfaces.CourseCapsule
{
    public interface ICourseCapsuleGetService
    {
        Task<AllCourseCapsulesResponse> GetAllCourseCapsulesAsync(int LoginID, int SubjectID, string AppType);
        Task<AllCourseCapsulesForASubjectResponse> GetAllCourseCapsulesForASubjectAsync(int LoginID, int SubjectID, string AppType);
        Task<FoldersForACourseCapsuleResponse> GetAllCourseCapsuleFoldersForACourseCapsuleAsync(int CourseCapsuleID);

        Task<PublicCourseCapsuleByDomainResponse> GetAllPublicCourseCapsuleByDomainAndSubjectAsync(int DomainID, int SubjectID);

        Task<LPStudentLicensesByCourseCapsuleResponse> GetAllLPStudentLicensesByCourseCapsuleAsync(int LoginID, int CourseID, int CourseCapsuleID);

        Task<LPCourseLicensesResponse> GetAllLPCourseLicensesAsync(int LoginID, int CourseID, int CourseCapsuleID);

        Task<CourseCapsuleForReorderResponse> GetCourseCapsuleForRecorderAsync(int LoginID, int SubjectID, int CourseCapsuleID);
    }
}
