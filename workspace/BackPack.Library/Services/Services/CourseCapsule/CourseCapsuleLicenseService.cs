
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Services.Interfaces.CourseCapsule;

namespace BackPack.Library.Services.Services.CourseCapsule
{
    public class CourseCapsuleLicenseService(
        ICreateCourseCapsuleLicenseRepository createCourseCapsuleLicenseRepository
        ) : ICourseCapsuleLicenseService
    {
        #region CreateCourseCapsuleLicenseAsync
        public async Task<BaseResponse> CreateCourseCapsuleLicenseAsync(CreateCourseCapsuleLicenseRequest request)
        {
            BaseResponse response = await createCourseCapsuleLicenseRepository.CreateCourseCapsuleLicenseAsync(request);

            return response;
        }
        #endregion
    }
}
