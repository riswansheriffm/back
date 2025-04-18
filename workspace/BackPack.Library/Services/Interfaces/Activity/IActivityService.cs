using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.Activity;
using BackPack.Library.Responses.Activity;

namespace BackPack.Library.Services.Interfaces.Activity
{
    public interface IActivityService
    {
        Task<BaseResponse> SaveStudentActivityAsync(StudentActivityRequest request);

        Task<BaseResponse> SaveTeacherFeedbackOnActivityAsync(TeacherFeedbackOnActivityRequest request);

        Task<BackpackActivityForStudentResponse> BackpackActivityForStudentAsync(int StudentID, int ContentID, int ParentID);
    }
}
