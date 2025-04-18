using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface IUpdateCourseCapsuleFolderRepository
    {
        Task<BaseResponse> UpdateCourseCapsuleFolderAsync(UpdateCourseCapsuleFolderRequest request);
    }
}
