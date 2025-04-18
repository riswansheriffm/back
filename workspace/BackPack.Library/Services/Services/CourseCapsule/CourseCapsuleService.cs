using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Repositories.Interfaces.Global;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using Microsoft.AspNetCore.Http;

namespace BackPack.Library.Services.Services.CourseCapsule
{
    public class CourseCapsuleService(
        IGlobalRepository globalRepository, 
        ISaveCourseCapsuleRepository saveCourseCapsuleRepository,
        ICreateCourseCapsuleRepository createCourseCapsuleRepository, 
        IEditCourseCapsuleRepository editCourseCapsuleRepository,
        IDeleteCourseCapsuleRepository deleteCourseCapsuleRepository, 
        IEnableCourseCapsuleRepository enableCourseCapsuleRepository,         
        IUpdateCourseCapsuleFolderRepository updateCourseCapsuleFolderRepository,
        ISaveCourseCapsuleFolderRepository saveCourseCapsuleFolderRepository,
        IUpdateCourseCapsulePodRepository updateCourseCapsulePodRepository,
        IUpdateCourseCapsuleActivityRepository updateCourseCapsuleActivityRepository,
        IPublishCourseCapsuleRepository publishCourseCapsuleRepository
        ) : ICourseCapsuleService
    {
        #region SaveCourseCapsuleAsync
        public async Task<SaveCourseCapsuleResponse> SaveCourseCapsuleAsync(SaveCourseCapsuleRequest request)
        {
            SaveCourseCapsuleResponse response = new();

            #region Check Domain
            var courseCapsuleCount = await globalRepository.CheckCourseCapsuleByID(request.DomainID,request.SubjectID,request.CourseCapsuleID,request.CourseCapsuleName!);

            if (courseCapsuleCount > 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CourseCapsuleMessage.SaveCourseCapsuleDuplicate;
                return response; 
            }
            #endregion

            SaveCourseCapsuleRequest saveCourseCapsuleRequest = new()
            {
                DomainID = request.DomainID,
                SubjectID = request.SubjectID,
                LoginID = request.LoginID,
                CourseCapsuleDesc = request.CourseCapsuleDesc,
                CourseCapsuleName = request.CourseCapsuleName,
                ImageURL = request.ImageURL,
                CourseCapsuleID = request.CourseCapsuleID,
                AppType = request.AppType 
            };

             response = await saveCourseCapsuleRepository.SaveCourseCapsuleAsync(saveCourseCapsuleRequest);

            return response;
        }
        #endregion

        #region CreateCourseCapsuleAsync
        public async Task<BaseResponse> CreateCourseCapsuleAsync(CreateCourseCapsuleRequest request)
        {
            BaseResponse response = new();

            #region Check Course Capsule
            var courseCapsuleCount = await globalRepository.CheckCourseCapsuleByID(request.DomainID, request.SubjectID, request.CourseCapsuleID, request.CourseCapsuleName!);

            if (courseCapsuleCount > 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CourseCapsuleMessage.CreateCourseCapsuleDuplicate;
                return response;
            }
            #endregion
             
            CreateCourseCapsuleRequest createCourseCapsuleRequest = new()
            {
               CourseCapsuleName = request.CourseCapsuleName,
               CourseCapsuleDesc= request.CourseCapsuleDesc,
               ImageURL = request.ImageURL,
               CourseCapsuleID = request.CourseCapsuleID,
               LoginID = request.LoginID,
               DomainID = request.DomainID,
               SubjectID = request.SubjectID,
               AppType = request.AppType,
               LessonPods = request.LessonPods,

            };

            response = await createCourseCapsuleRepository.CreateCourseCapsuleAsync(createCourseCapsuleRequest);

            return response;
        }
        #endregion

        #region EditCourseCapsuleAsync
        public async Task<BaseResponse> EditCourseCapsuleAsync(EditCourseCapsuleRequest request)
        {
            BaseResponse response = new();

            #region Check Domain
            var courseCapsuleCount = await globalRepository.CheckCourseCapsuleByID(request.DomainID, request.SubjectID, request.CourseCapsuleID, request.CourseCapsuleName!);

            if (courseCapsuleCount > 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CourseCapsuleMessage.EditCourseCapsuleDuplicate;
                return response;
            }
            #endregion

            EditCourseCapsuleRequest editCourseCapsuleRequest = new()
            {
                CourseCapsuleName = request.CourseCapsuleName,
                CourseCapsuleDesc = request.CourseCapsuleDesc,
                ImageURL = request.ImageURL,
                CourseCapsuleID = request.CourseCapsuleID,
                LoginID = request.LoginID,
                DomainID = request.DomainID,
                SubjectID = request.SubjectID, 
                LessonPods = request.LessonPods,

            };

            response = await editCourseCapsuleRepository.EditCourseCapsuleAsync(editCourseCapsuleRequest);

            return response;
        }
        #endregion      

        #region DeleteCourseCapsuleAsync
        public async Task<BaseResponse> DeleteCourseCapsuleAsync(DeleteCourseCapsuleRequest request)
        {
            BaseResponse response = await deleteCourseCapsuleRepository.DeleteCourseCapsuleAsync(request);

            return response;
        }
        #endregion

        #region EnableCourseCapsuleAsync
        public async Task<BaseResponse> EnableCourseCapsuleAsync(DeleteCourseCapsuleRequest request)
        {
            BaseResponse response = await enableCourseCapsuleRepository.EnableCourseCapsuleAsync(request);

            return response;
        }
        #endregion
        
        #region SaveCourseCapsuleFolderAsync
        public async Task<SaveCourseCapsuleFolderResponse> SaveCourseCapsuleFolderAsync(SaveCourseCapsuleFolderRequest request)
        {
            SaveCourseCapsuleFolderResponse response = await saveCourseCapsuleFolderRepository.SaveCourseCapsuleFolderAsync(request);

            return response;
        }
        #endregion

        #region UpdateCourseCapsuleFolderAsync
        public async Task<BaseResponse> UpdateCourseCapsuleFolderAsync(UpdateCourseCapsuleFolderRequest request)
        {
            BaseResponse response = await updateCourseCapsuleFolderRepository.UpdateCourseCapsuleFolderAsync(request);

            return response;
        }
        #endregion

        #region UpdateCourseCapsulePodAsync
        public async Task<BaseResponse> UpdateCourseCapsulePodAsync(UpdateCourseCapsulePodRequest request)
        {
            BaseResponse response = await updateCourseCapsulePodRepository.UpdateCourseCapsulePodAsync(request);

            return response;
        }
        #endregion

        #region UpdateCourseCapsuleActivityAsync
        public async Task<BaseResponse> UpdateCourseCapsuleActivityAsync(UpdateCourseCapsuleActivityRequest request)
        {
            BaseResponse response = await updateCourseCapsuleActivityRepository.UpdateCourseCapsuleActivityAsync(request);

            return response;
        }
        #endregion

        #region PublishCourseCapsuleAsync
        public async Task<PublishCourseCapsuleResponse> PublishCourseCapsuleAsync(PublishCourseCapsuleRequest request)
        {
            PublishCourseCapsuleResponse response = await publishCourseCapsuleRepository.PublishCourseCapsuleAsync(request);

            return response;
        }
        #endregion

    }
}
