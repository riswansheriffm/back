using BackPack.Library.Requests.Master.Course;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Course;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface ICourseService
    {
        Task<CourseResponse> GetCourseAsync(int CourseID);

        Task<AllCourseResponse> GetAllCourseAsync(int SchoolID);        

        Task<AllCoursesByDomainResponse> GetAllCoursesByDomainAsync(int DomainID);        

        Task<StudentListForACourseResponse> StudentListForACourseAsync(int CourseID);

        Task<AllCoursesForASubjectResponse> AllCoursesForASubjectAsync(int SubjectID, int LoginID);

        Task<BaseResponse> CreateCourseAsync(CreateCourseRequest request);

        Task<BaseResponse> UpdateCourseAsync(UpdateCourseRequest request);

        Task<BaseResponse> DeleteCourseAsync(DeleteCourseRequest request);

        Task<BaseResponse> CreateCourseBulkAsync(CreateCourseBulkRequest request);

        Task<BaseResponse> MapTeacherToCourseAsync(BulkMapTeacherToCourseRequest request);
    }
} 
