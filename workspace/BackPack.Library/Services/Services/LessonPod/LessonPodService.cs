using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.LessonPod;
using BackPack.Library.Repositories.Repositories.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class LessonPodService(
        ICreateLessonPodRepository createLessonPodRepository,
        IUpdateLessonPodPropertyRepository updateLessonPodPropertyRepository,
        ICopyLessonPodRepository copyLessonPodRepository,
        IDeleteLessonPodRepository deleteLessonPodRepository,
        IUpdateLessonInLessonPodRepository updateLessonInLessonPodRepository,
        ICreatePreviewLessonPodActivityRepository createPreviewLessonPodActivityRepository,
        ICreateLessonPodSlideTemplateRepository createLessonPodSlideTemplateRepository,
        IDeleteLessonPodSlideTemplateRepository deleteLessonPodSlideTemplateRepository,
        IPendingLessonPodRepository pendingLessonPodRepository,
        ICompletedLessonPodRepository completedLessonPodRepository,
        ILessonPodSummaryRepository lessonPodSummaryRepository,
        ILessonPodDetailsRepository lessonPodDetailsRepository,
        IAllDistributedLessonpodsByTeacherRepository allDistributedLessonpodsByTeacherRepository,
        IDistributeLessonPodRepository distributeLessonPodRepository,
        IDistributedLessonPodRepository distributedLessonPodRepository,
        IAllMyLessonPodsByLessonRepository allMyLessonsByLessonRepository,
        ICRLessonUnitDetailsRepository crLessonUnitDetailsRepository,
        ILessonPodByIDRepository lessonPodByIDRepository,
        IAllMyLessonPodsRepository allMyLessonPodsRepository,
        IAllActivitiesByLessonPodRepository allActivitiesByLessonPodRepository,
        IAllSlideTemplateRepository allSlideTemplateRepository,
        ICreateLessonRepository createLessonRepository,
        IDeleteLessonRepository deleteLessonRepository,
        IUpdateLessonRepository updateLessonRepository,
        IGetLessonRepository getLessonRepository,
        IStudioPreviewActivityBySlideRepository studioPreviewActivityBySlideRepository,
        IStudioPreviewActivitiesByLessonPodRepository studioPreviewActivitiesByLessonPodRepository
        ) : ILessonPodService
    {
        #region CreateLessonPodAsync
        public async Task<CreateLessonPodResponse> CreateLessonPodAsync(CreateLessonPodRequest request)
        {
            CreateLessonPodResponse response = await createLessonPodRepository.CreateLessonPodAsync(request);

            return response;
        }
        #endregion

        #region UpdateLessonPodPropertiesAsync
        public async Task<BaseResponse> UpdateLessonPodPropertiesAsync(LessonPodPropertiesRequest request)
        {
            BaseResponse response = await updateLessonPodPropertyRepository.UpdateLessonPodPropertiesAsync(request);

            return response;
        }
        #endregion

        #region CopyLessonPodAsync
        public async Task<BaseResponse> CopyLessonPodAsync(CopyLessonPodRequest request)
        {
            BaseResponse response = await copyLessonPodRepository.CopyLessonPodAsync(request);

            return response;
        }
        #endregion

        #region DeleteLessonPodAsync
        public async Task<BaseResponse> DeleteLessonPodAsync(DeleteLessonPodRequest request)
        {
            BaseResponse response = await deleteLessonPodRepository.DeleteLessonPodAsync(request);

            return response;
        }
        #endregion

        #region UpdateLessonInLessonPodAsync
        public async Task<BaseResponse> UpdateLessonInLessonPodAsync(UpdateLessonInLessonPodRequest request)
        {
            BaseResponse response = await updateLessonInLessonPodRepository.UpdateLessonInLessonPodAsync(request);

            return response;
        }
        #endregion

        #region CreatePreviewLessonPodActivityAsync
        public async Task<BaseResponse> CreatePreviewLessonPodActivityAsync(CreatePreviewLessonPodActivityRequest request)
        {
            BaseResponse response = await createPreviewLessonPodActivityRepository.CreatePreviewLessonPodActivityAsync(request); 
            
            return response;
        }
        #endregion

        #region CreateLessonPodSlideTemplateAsync
        public async Task<CreateLessonPodSlideTemplateResponse> CreateLessonPodSlideTemplateAsync(CreateLessonPodSlideTemplateRequest request)
        {
            CreateLessonPodSlideTemplateResponse response = await createLessonPodSlideTemplateRepository.CreateLessonPodSlideTemplateAsync(request);

            return response;
        }
        #endregion

        #region DeleteLessonPodSlideTemplateAsync
        public async Task<BaseResponse> DeleteLessonPodSlideTemplateAsync(DeleteLessonPodSlideTemplateRequest request)
        {
            BaseResponse response = await deleteLessonPodSlideTemplateRepository.DeleteLessonPodSlideTemplateAsync(request);

            return response;
        }
        #endregion

        #region PendingLessonPodsForAStudentAsync
        public async Task<PendingLessonPodsForAStudentResponse> PendingLessonPodsForAStudentAsync(int StudentID, int CourseID, int ParentID, int LessonID)
        {
            PendingLessonPodsForAStudentResponse response = await pendingLessonPodRepository.PendingLessonPodsForAStudentAsync(StudentID, CourseID, ParentID, LessonID);

            return response;
        }
        #endregion

        #region CompletedLessonPodsByLessonAsync
        public async Task<CompletedLessonPodsByLessonResponse> CompletedLessonPodsByLessonAsync(int StudentID, int LessonID, int ParentID, int ChapterID)
        {
            CompletedLessonPodsByLessonResponse response = await completedLessonPodRepository.CompletedLessonPodsByLessonAsync(StudentID, LessonID, ParentID, ChapterID);

            return response;
        }
        #endregion

        #region LessonPodSummaryForAStudentAsync
        public async Task<LessonPodSummaryForAStudentResponse> LessonPodSummaryForAStudentAsync(int StudentID, int LessonUnitDistID)
        {
            LessonPodSummaryForAStudentResponse response = await lessonPodSummaryRepository.LessonPodSummaryForAStudentAsync(StudentID, LessonUnitDistID);

            return response;
        }
        #endregion

        #region LessonPodDetailsForAStudentAsync
        public async Task<LessonPodDetailsForAStudentResponse> LessonPodDetailsForAStudentAsync(int StudentID, int LessonUnitDistID, int ParentID)
        {
            LessonPodDetailsForAStudentResponse response = await lessonPodDetailsRepository.LessonPodDetailsForAStudentAsync(StudentID, LessonUnitDistID, ParentID);

            return response;
        }
        #endregion

        #region AllDistributedLessonpodsByTeacherAsync
        public async Task<AllDistributedLessonpodsByTeacherResponse> AllDistributedLessonpodsByTeacherAsync(int LoginID, int LessonUnitID)
        {
            AllDistributedLessonpodsByTeacherResponse response = await allDistributedLessonpodsByTeacherRepository.AllDistributedLessonpodsByTeacherAsync(LoginID, LessonUnitID);

            return response;
        }
        #endregion

        #region DistributeLessonPodAsync
        public async Task<DistributeLessonPodResponse> DistributeLessonPodAsync(int LessonUnitID, string LessonType)
        {
            DistributeLessonPodResponse response = await distributeLessonPodRepository.DistributeLessonPodAsync(LessonUnitID, LessonType);

            return response;
        }
        #endregion

        #region DistributedLessonPodAsync
        public async Task<DistributedLessonPodResponse> DistributedLessonPodAsync(int LessonUnitDistID)
        {
            DistributedLessonPodResponse response = await distributedLessonPodRepository.DistributedLessonPodAsync(LessonUnitDistID);

            return response;
        }
        #endregion

        #region AllMyLessonPodsByLessonAsync
        public async Task<AllMyLessonPodsByLessonResponse> AllMyLessonPodsByLessonAsync(int LessonID, int LoginID)
        {
            AllMyLessonPodsByLessonResponse response = await allMyLessonsByLessonRepository.AllMyLessonPodsByLessonAsync(LessonID, LoginID);

            return response;
        }
        #endregion

        #region CRLessonUnitDetailsAsync
        public async Task<CRLessonUnitDetailsResponse> CRLessonUnitDetailsAsync(int LessonUnitDistID)
        {
            CRLessonUnitDetailsResponse response = await crLessonUnitDetailsRepository.CRLessonUnitDetailsAsync(LessonUnitDistID);

            return response;
        }
        #endregion

        #region LessonPodAsync
        public async Task<LessonPodResponse> LessonPodAsync(int LessonUnitID, int AuthorID)
        {
            LessonPodResponse response = await lessonPodByIDRepository.LessonPodAsync(LessonUnitID, AuthorID);

            return response;
        }
        #endregion

        #region AllMyLessonPodsAsync
        public async Task<AllMyLessonPodsResponse> AllMyLessonPodsAsync(int LessonID, int AuthorID)
        {
            AllMyLessonPodsResponse response = await allMyLessonPodsRepository.AllMyLessonPodsAsync(LessonID, AuthorID);

            return response;
        }
        #endregion

        #region AllActivitiesByLessonPodAsync
        public async Task<AllActivitiesByLessonPodResponse> AllActivitiesByLessonPodAsync(int LessonUnitID, int LoginID)
        {
            AllActivitiesByLessonPodResponse response = await allActivitiesByLessonPodRepository.AllActivitiesByLessonPodAsync(LessonUnitID, LoginID);

            return response;
        }
        #endregion

        #region AllSlideTemplateAsync
        public async Task<AllSlideTemplateResponse> AllSlideTemplateAsync(int DomainID, int LoginID)
        {
            AllSlideTemplateResponse response = await allSlideTemplateRepository.AllSlideTemplateAsync(DomainID, LoginID);

            return response;
        }
        #endregion

        #region CreateLessonAsync
        public async Task<BaseResponse> CreateLessonAsync(CreateLessonRequest request)
        {
            BaseResponse response = await createLessonRepository.CreateLessonAsync(request);

            return response;
        }
        #endregion

        #region DeleteLessonAsync
        public async Task<BaseResponse> DeleteLessonAsync(DeleteLessonRequest request)
        {
            BaseResponse response = await deleteLessonRepository.DeleteLessonAsync(request);

            return response;
        }
        #endregion

        #region UpdateLessonAsync
        public async Task<BaseResponse> UpdateLessonAsync(UpdateLessonRequest request)  
        {
            BaseResponse response = await updateLessonRepository.UpdateLessonAsync(request);

            return response;
        }
        #endregion

        #region GetLessonAsync
        public async Task<LessonResponse> GetLessonAsync(int LessonID)
        {
            LessonResponse response = await getLessonRepository.GetLessonAsync(LessonID);

            return response;
        }
        #endregion

        #region GetLPStudioPreviewActivityBySlideAsync
        public async Task<StudioPreviewActivityBySlideResponse> GetLPStudioPreviewActivityBySlideAsync(int LoginID, int LessonUnitID, string SlideID, string PreviewMode)
        {
            StudioPreviewActivityBySlideResponse response = await studioPreviewActivityBySlideRepository.GetLPStudioPreviewActivityBySlideAsync(LoginID, LessonUnitID, SlideID, PreviewMode);

            return response;
        }
        #endregion

        #region GetLPStudioPreviewActivityBySlideAsync
        public async Task<StudioPreviewActivitiesByLessonPodResponse> GetLPStudioPreviewActivitiesByLessonPodAsync(int LoginID, int LessonUnitID, string PreviewMode)
        {
            StudioPreviewActivitiesByLessonPodResponse response = await studioPreviewActivitiesByLessonPodRepository.GetLPStudioPreviewActivitiesByLessonPodAsync(LoginID, LessonUnitID, PreviewMode);

            return response;
        }
        #endregion
    }
}
