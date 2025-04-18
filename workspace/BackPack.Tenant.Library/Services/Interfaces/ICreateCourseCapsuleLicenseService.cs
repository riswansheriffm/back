
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface ICreateCourseCapsuleLicenseService
    {
        Task<BaseResponse> CreateCourseCapsuleLicenseAsync(CreateCourseCapsuleLicenseRequest request);
    }
}
