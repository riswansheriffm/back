using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IDeleteUserRepository
    {
        Task<BaseResponse> DeleteUserAsync(DeleteUserRequest request);
    }
}
