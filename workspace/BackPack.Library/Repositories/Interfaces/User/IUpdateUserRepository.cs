using BackPack.Library.Requests.User;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IUpdateUserRepository
    {
        Task<BaseResponse> UpdateUserAsync(UpdateUserRequest request);
    }
}
