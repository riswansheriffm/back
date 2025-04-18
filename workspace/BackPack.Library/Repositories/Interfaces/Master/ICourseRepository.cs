using BackPack.Library.Responses.Master.Course;

namespace BackPack.Library.Repositories.Interfaces.Master
{
    public interface ICourseRepository
    {
        Task<CourseResponse> GetCourseAsync(int CourseID);

        Task<AllCourseResponse> GetAllCourseAsync(int SchoolID);        

        Task<AllCoursesByDomainResponse> GetAllCoursesByDomainAsync(int DomainID);
        
        Task<StudentListForACourseResponse> StudentListForACourseAsync(int CourseID);
        
        Task<AllCoursesForASubjectResponse> AllCoursesForASubjectAsync(int SubjectID, int LoginID);
    }
}
