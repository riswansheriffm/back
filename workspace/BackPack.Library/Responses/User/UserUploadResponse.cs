
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class UserUploadResponse : BaseResponse
    {
        public List<UserListUploadResponse> Messages { get; set; } = new List<UserListUploadResponse>();
    }
}
