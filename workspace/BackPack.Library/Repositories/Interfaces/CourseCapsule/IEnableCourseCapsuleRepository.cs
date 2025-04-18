using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface IEnableCourseCapsuleRepository
    {
        Task<BaseResponse> EnableCourseCapsuleAsync(DeleteCourseCapsuleRequest request);
    }
}
