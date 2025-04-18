using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IUserUploadRepository
    {
        Task<UserUploadQueryResponse> CreateUserUploadAsync(UserUploadQueryRequest request);
    }
}
 