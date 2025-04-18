using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.LessonPod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.LessonPod
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.LessonPod)]
    public class LessonpodStudentController(
        IDBLogService dbLogService,
        ILessonpodStudentGetService lessonpodStudentGetService
        ) : BaseController(dbLogService)
    {
        #region GetLessonUnitsForAStudent
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetLessonUnitsForAStudent")]
        [OperationOrder(42)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.LessonUnitsForAStudentSummary,
            Description = LessonPodMessage.LessonUnitsForAStudentDescription
        )]
        [ProducesResponseType(typeof(PendingLessonPodsForAStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonUnitsForAStudent([Required][FromHeader] int StudentID, [Required][FromHeader] int CourseID, [Required][FromHeader] int LessonID, [Required][FromHeader] int ChapterID, [FromHeader] int ParentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", CourseID: " + CourseID + ", " + ControllerConstant.ParentId + ParentID + ", LessonID: " + LessonID + ", ChapterID: " + ChapterID
            });

            var response = await lessonpodStudentGetService.GetLessonpodsForAStudentAsync(studentId: StudentID, courseId: CourseID, lessonId: LessonID, chapterId: ChapterID, parentId: ParentID);

            await UpdateDBLogAsync(new DBLogRequest
            {
                ServiceLogID = guid,
                ServiceResponse = JsonSerializer.Serialize(response),
                ServiceStatus = (!response.Success) ? CommonMessage.ServiceStatusFail : CommonMessage.ServiceStatusSuccess,
                LogMessage = response.StatusMessage!,
                ExceptionType = response.ExceptionType!,
                ExceptionMessage = response.ExceptionMessage!,
                Success = response.Success
            });

            #region Response
            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
            #endregion
        }
        #endregion
    }
}
