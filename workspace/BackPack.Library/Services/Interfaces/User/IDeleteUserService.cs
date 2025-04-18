
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface IDeleteUserService
    {
        Task<BaseResponse> DeleteStudentAsync(DeleteStudentRequest request);
    }
}
