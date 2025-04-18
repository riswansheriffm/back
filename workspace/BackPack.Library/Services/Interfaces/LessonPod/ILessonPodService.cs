using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface ILessonPodService
    {
        Task<CreateLessonPodResponse> CreateLessonPodAsync(CreateLessonPodRequest request);

        Task<BaseResponse> UpdateLessonPodPropertiesAsync(LessonPodPropertiesRequest request);

        Task<BaseResponse> CopyLessonPodAsync(CopyLessonPodRequest request);

        Task<BaseResponse> DeleteLessonPodAsync(DeleteLessonPodRequest request);

        Task<BaseResponse> UpdateLessonInLessonPodAsync(UpdateLessonInLessonPodRequest request);

        Task<BaseResponse> CreatePreviewLessonPodActivityAsync(CreatePreviewLessonPodActivityRequest request);

        Task<CreateLessonPodSlideTemplateResponse> CreateLessonPodSlideTemplateAsync(CreateLessonPodSlideTemplateRequest request);

        Task<BaseResponse> DeleteLessonPodSlideTemplateAsync(DeleteLessonPodSlideTemplateRequest request);

        Task<PendingLessonPodsForAStudentResponse> PendingLessonPodsForAStudentAsync(int StudentID, int CourseID, int ParentID, int LessonID);

        Task<CompletedLessonPodsByLessonResponse> CompletedLessonPodsByLessonAsync(int StudentID, int LessonID, int ParentID, int ChapterID);

        Task<LessonPodSummaryForAStudentResponse> LessonPodSummaryForAStudentAsync(int StudentID, int LessonUnitDistID);

        Task<LessonPodDetailsForAStudentResponse> LessonPodDetailsForAStudentAsync(int StudentID, int LessonUnitDistID, int ParentID);

        Task<AllDistributedLessonpodsByTeacherResponse> AllDistributedLessonpodsByTeacherAsync(int LoginID, int LessonUnitID);

        Task<DistributeLessonPodResponse> DistributeLessonPodAsync(int LessonUnitID, string LessonType);

        Task<DistributedLessonPodResponse> DistributedLessonPodAsync(int LessonUnitDistID);

        Task<AllMyLessonPodsByLessonResponse> AllMyLessonPodsByLessonAsync(int LessonID, int LoginID);

        Task<CRLessonUnitDetailsResponse> CRLessonUnitDetailsAsync(int LessonUnitDistID);

        Task<LessonPodResponse> LessonPodAsync(int LessonUnitID, int AuthorID);

        Task<AllMyLessonPodsResponse> AllMyLessonPodsAsync(int LessonID, int AuthorID);

        Task<AllActivitiesByLessonPodResponse> AllActivitiesByLessonPodAsync(int LessonUnitID, int LoginID);

        Task<AllSlideTemplateResponse> AllSlideTemplateAsync(int DomainID, int LoginID);

        Task<BaseResponse> CreateLessonAsync(CreateLessonRequest request);

        Task<BaseResponse> DeleteLessonAsync(DeleteLessonRequest request);

        Task<BaseResponse> UpdateLessonAsync(UpdateLessonRequest request);

        Task<LessonResponse> GetLessonAsync(int LessonID);

        Task<StudioPreviewActivityBySlideResponse> GetLPStudioPreviewActivityBySlideAsync(int LoginID, int LessonUnitID, string SlideID, string PreviewMode);

        Task<StudioPreviewActivitiesByLessonPodResponse> GetLPStudioPreviewActivitiesByLessonPodAsync(int LoginID, int LessonUnitID, string PreviewMode);
    }
}
 