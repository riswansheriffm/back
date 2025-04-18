using BackPack.Library.Requests.User;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.User
{
    public interface IUpdateStudentRepository
    {
        Task<BaseResponse> UpdateStudentAsync(UpdateStudentRequest request);
    }
}
