using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface ISaveCourseCapsuleRepository 
    {
        Task<SaveCourseCapsuleResponse> SaveCourseCapsuleAsync(SaveCourseCapsuleRequest request);

    }
}
