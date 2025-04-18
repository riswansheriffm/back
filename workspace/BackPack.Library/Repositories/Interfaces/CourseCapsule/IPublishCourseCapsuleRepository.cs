
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;

namespace BackPack.Library.Repositories.Interfaces.CourseCapsule
{
    public interface IPublishCourseCapsuleRepository
    {
        Task<PublishCourseCapsuleResponse> PublishCourseCapsuleAsync(PublishCourseCapsuleRequest request);
    }
}
