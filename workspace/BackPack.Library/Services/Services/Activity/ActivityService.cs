using BackPack.Dependency.Library.Responses;
using BackPack.Library.Repositories.Interfaces.Activity;
using BackPack.Library.Requests.Activity;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Services.Interfaces.Activity;

namespace BackPack.Library.Services.Services.Activity
{    
    public class ActivityService(
        IActivitySaveRepository activitySaveRepository,
        IActivityRepository activityRepository, 
        ISaveTeacherFeedbackOnActivityRepository saveTeacherFeedbackOnActivityRepository
        ) : IActivityService
    {
        #region SaveStudentActivityAsync
        public async Task<BaseResponse> SaveStudentActivityAsync(StudentActivityRequest request)
        {
            BaseResponse response = await activitySaveRepository.SaveStudentActivityAsync(request);

            return response;
        }
        #endregion

        #region SaveTeacherFeedbackOnActivityAsync
        public async Task<BaseResponse> SaveTeacherFeedbackOnActivityAsync(TeacherFeedbackOnActivityRequest request)
        {
            BaseResponse response = await saveTeacherFeedbackOnActivityRepository.SaveTeacherFeedbackOnActivityAsync(request);

            return response;
        }
        #endregion

        #region BackpackActivityForStudentAsync
        public async Task<BackpackActivityForStudentResponse> BackpackActivityForStudentAsync(int StudentID, int ContentID, int ParentID)
        {
            BackpackActivityForStudentResponse response = await activityRepository.BackpackActivityForStudentAsync(StudentID, ContentID, ParentID);

            return response;
        }
        #endregion
    }
}
