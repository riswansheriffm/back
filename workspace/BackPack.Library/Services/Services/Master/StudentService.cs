using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Responses.Master.Student;
using BackPack.Library.Services.Interfaces.Master;

namespace BackPack.Library.Services.Services.Master
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        #region GetStudentAsync
        public async Task<StudentResponse> GetStudentAsync(int LoginID)
        {
            StudentResponse response = await studentRepository.GetStudentAsync(LoginID);

            return response;
        }
        #endregion 

        #region GetAllStudentAsync
        public async Task<AllStudentResponse> GetAllStudentAsync(int DomainID, int SchoolID)
        {
            AllStudentResponse response = await studentRepository.GetAllStudentAsync(DomainID, SchoolID);

            return response;
        }
        #endregion 
    }
}
