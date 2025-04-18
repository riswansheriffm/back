using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Global;
using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Repositories.Interfaces.Master.Course;
using BackPack.Library.Requests.Master.Course;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Course;
using BackPack.Library.Services.Interfaces.Master;
using Microsoft.AspNetCore.Http;
using BackPack.Dependency.Library.Messages;

namespace BackPack.Library.Services.Services.Master
{
    public class CourseService(
        ICourseRepository courseRepository,
        ICreateCourseRepository createCourseRepository,
        IUpdateCourseRepository updateCourseRepository,
        IDeleteCourseRepository deleteCourseRepository,
        ICreateCourseBulkRepository createCourseBulkRepository,
        IGlobalRepository globalRepository,
        IMapTeacherToCourseRepository mapTeacherToCourseRepository
        ) : ICourseService
    {
        #region GetCourseAsync
        public async Task<CourseResponse> GetCourseAsync(int CourseID)
        {
            CourseResponse response = await courseRepository.GetCourseAsync(CourseID);

            return response;
        }
        #endregion

        #region GetAllCourseAsync
        public async Task<AllCourseResponse> GetAllCourseAsync(int SchoolID)
        {
            AllCourseResponse response = await courseRepository.GetAllCourseAsync(SchoolID);

            return response;
        }
        #endregion
        
        #region GetAllCoursesByDomainAsync
        public async Task<AllCoursesByDomainResponse> GetAllCoursesByDomainAsync(int DomainID)
        {
            AllCoursesByDomainResponse response = await courseRepository.GetAllCoursesByDomainAsync(DomainID);

            return response;
        }
        #endregion
        
        #region StudentListForACourseAsync        
        public async Task<StudentListForACourseResponse> StudentListForACourseAsync(int CourseID)
        {
            StudentListForACourseResponse response = await courseRepository.StudentListForACourseAsync(CourseID);

            return response;
        }
        #endregion

        #region AllCoursesForASubjectAsync        
        public async Task<AllCoursesForASubjectResponse> AllCoursesForASubjectAsync(int SubjectID, int LoginID)
        {
            AllCoursesForASubjectResponse response = await courseRepository.AllCoursesForASubjectAsync(SubjectID, LoginID);

            return response;
        }
        #endregion 

        #region CreateCourseAsync
        public async Task<BaseResponse> CreateCourseAsync(CreateCourseRequest request)
        {
            BaseResponse response = await createCourseRepository.CreateCourseAsync(request);

            return response;
        }
        #endregion 

        #region UpdateCourseAsync
        public async Task<BaseResponse> UpdateCourseAsync(UpdateCourseRequest request)
        {
            BaseResponse response = await updateCourseRepository.UpdateCourseAsync(request);

            return response;
        }
        #endregion 

        #region DeleteCourseAsync
        public async Task<BaseResponse> DeleteCourseAsync(DeleteCourseRequest request)
        {
            BaseResponse response = await deleteCourseRepository.DeleteCourseAsync(request);

            return response;
        }
        #endregion 

        #region CreateCourseBulkAsync
        public async Task<BaseResponse> CreateCourseBulkAsync(CreateCourseBulkRequest request)
        {
            BaseResponse response = new();

            #region Check School
            var schoolCount = await globalRepository.CheckSchoolByID(request.SchoolID);

            if (schoolCount == 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.SchoolNotFound;
                return response;
            }
            #endregion

             response = await createCourseBulkRepository.CreateCourseBulkAsync(request);

            return response;
        }
        #endregion

        #region MapTeacherToCourseAsync
        public async Task<BaseResponse> MapTeacherToCourseAsync(BulkMapTeacherToCourseRequest request)
        {
            BaseResponse response = new();

            #region Check School
            var domainCount = await globalRepository.CheckDomainByID(request.DomainID);
             
            if (domainCount == 0)
            {
                response.MessageID = CommonMessage.FailID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = UserMessage.DomainNotFound;
                return response;
            }
            #endregion

            response = await mapTeacherToCourseRepository.MapTeacherToCourseAsync(request);

            return response;
        }
        #endregion 
    }
} 
