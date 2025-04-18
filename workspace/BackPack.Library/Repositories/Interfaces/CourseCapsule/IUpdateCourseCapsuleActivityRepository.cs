using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface IUpdateCourseCapsuleActivityRepository
    {
        Task<BaseResponse> UpdateCourseCapsuleActivityAsync(UpdateCourseCapsuleActivityRequest request);
    }
}
