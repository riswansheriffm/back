

using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.CourseCapsule;

namespace BackPack.Library.Services.Interfaces.CourseCapsule
{
    public interface ICourseCapsuleLicenseService
    {
        Task<BaseResponse> CreateCourseCapsuleLicenseAsync(CreateCourseCapsuleLicenseRequest request);
    }
}
