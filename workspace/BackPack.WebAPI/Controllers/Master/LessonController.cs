using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Lesson;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BackPack.Dependency.Library.Swagger;
using BackPack.Dependency.Library.Messages;

namespace BackPack.WebAPI.Controllers.Master
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.Master)]
    public class LessonController(
        IDBLogService dbLogService,
        ILessonService lessonService
        ) : BaseController(dbLogService)
    {
        #region GetAllLessonsForASubject
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllLessonsForASubject")]
        [OperationOrder(78)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.AllLessonsForASubjectSummary,
            Description = MasterMessage.AllLessonsForASubjectDescription
        )]
        [ProducesResponseType(typeof(AllLessonsForASubjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLessonsForASubject([Required][FromHeader] int SubjectID, [Required][FromHeader] int ChapterID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + LoginID + ", SubjectID: " + SubjectID + ", ChapterID: " + ChapterID
            });

            var response = await lessonService.AllLessonsForASubjectAsync(SubjectID, ChapterID);

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
