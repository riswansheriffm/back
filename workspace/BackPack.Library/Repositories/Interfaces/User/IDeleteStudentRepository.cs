
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IDeleteStudentRepository
    {
        Task<BaseResponse> DeleteStudentAsync(DeleteStudentRequest request);
    }
}
