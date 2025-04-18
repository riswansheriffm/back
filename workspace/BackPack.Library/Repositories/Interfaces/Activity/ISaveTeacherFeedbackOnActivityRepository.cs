using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.Activity;

namespace BackPack.Library.Repositories.Interfaces.Activity
{
    public interface ISaveTeacherFeedbackOnActivityRepository
    {
        Task<BaseResponse> SaveTeacherFeedbackOnActivityAsync(TeacherFeedbackOnActivityRequest request);
    }
}
