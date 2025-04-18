using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;

namespace BackPack.Library.Services.Interfaces.CourseCapsule
{
    public interface ICourseCapsuleService
    {
        Task<SaveCourseCapsuleResponse> SaveCourseCapsuleAsync(SaveCourseCapsuleRequest request);

        Task<BaseResponse> CreateCourseCapsuleAsync(CreateCourseCapsuleRequest request);

        Task<BaseResponse> EditCourseCapsuleAsync(EditCourseCapsuleRequest request);

        Task<BaseResponse> DeleteCourseCapsuleAsync(DeleteCourseCapsuleRequest request);

        Task<BaseResponse> EnableCourseCapsuleAsync(DeleteCourseCapsuleRequest request);        

        Task<SaveCourseCapsuleFolderResponse> SaveCourseCapsuleFolderAsync(SaveCourseCapsuleFolderRequest request);

        Task<BaseResponse> UpdateCourseCapsuleFolderAsync(UpdateCourseCapsuleFolderRequest request);

        Task<BaseResponse> UpdateCourseCapsulePodAsync(UpdateCourseCapsulePodRequest request);

        Task<BaseResponse> UpdateCourseCapsuleActivityAsync(UpdateCourseCapsuleActivityRequest request);

        Task<PublishCourseCapsuleResponse> PublishCourseCapsuleAsync(PublishCourseCapsuleRequest request);

    }
}
