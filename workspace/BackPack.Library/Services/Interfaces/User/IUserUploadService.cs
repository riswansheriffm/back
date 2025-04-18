using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface IUserUploadService 
    {
        Task<UserUploadResponse> CreateUserUploadAsync(UserUploadRequest request);

        Task<BaseResponse> CreateUserAsync(UserRequest request);

        Task<BaseResponse> UpdateUserAsync(UpdateUserRequest request);

        Task<BaseResponse> DeleteUserAsync(DeleteUserRequest request);

        Task<BaseResponse> UpdateStudentAsync(UpdateStudentRequest request);      
    }
}
 