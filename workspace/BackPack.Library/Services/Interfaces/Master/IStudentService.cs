using BackPack.Library.Responses.Master.Student;

namespace BackPack.Library.Services.Interfaces.Master
{
    public interface IStudentService
    {
        Task<StudentResponse> GetStudentAsync(int LoginID);

        Task<AllStudentResponse> GetAllStudentAsync(int DomainID, int SchoolID);
    }
}
