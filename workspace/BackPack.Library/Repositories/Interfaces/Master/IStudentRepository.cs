using BackPack.Library.Responses.Master.Student;

namespace BackPack.Library.Repositories.Interfaces.Master
{
    public interface IStudentRepository
    {
        Task<StudentResponse> GetStudentAsync(int LoginID);

        Task<AllStudentResponse> GetAllStudentAsync(int DomainID, int SchoolID);
    }
}
