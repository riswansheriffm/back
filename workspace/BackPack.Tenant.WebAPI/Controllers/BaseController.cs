using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace BackPack.Tenant.WebAPI.Controllers
{
    public class BaseController(IDBLogService dbLogService) : Controller
    {
        #region CreateDBLogAsync        
        protected async ValueTask<Guid> CreateDBLogAsync(DBLogRequest request)
        {
            Guid guid = Guid.NewGuid();
            request.ServiceLogID = guid;
            request.LoginID = GlobalApplicationProperty.UserID;
            
            await dbLogService!.CreateRequestAsync(request);

            return guid;
        }
        #endregion

        #region ValidationResponse        
        protected async Task<BadRequestResponse> ValidationResponse(object validation, Guid guid)
        {
            var errorList = validation.ToString()!.Split("'");
            var message = CommonMessage.ValidationMessageLeft + errorList[1] + CommonMessage.ValidationMessageMiddle + validation.ToString();

            DBLogRequest request = new()
            {
                ServiceLogID = guid,
                ServiceStatus = CommonMessage.ServiceStatusFail,
                ServiceResponse = JsonSerializer.Serialize(validation),
                LogMessage = message,
                ExceptionType = CommonMessage.ExceptionTypeValidation
            };
            request.ServiceStatus = CommonMessage.ServiceStatusFail;
            request.LogMessage = message;
            request.ExceptionType = CommonMessage.ExceptionTypeValidation;
            
            Log.Warning("{Message}-{UserId}-{Guid}-{ErrorMessage}", message, GlobalApplicationProperty.UserID, guid, validation);
            await dbLogService!.UpdateRequestAsync(request);

            return new BadRequestResponse
            {
                MessageID = CommonMessage.InvalidParameterID,
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                StatusMessage = CommonMessage.BadRequestMessage
            };
        }
        #endregion

        #region UpdateDBLogAsync        
        protected async Task UpdateDBLogAsync(DBLogRequest request)
        {
            if (!request.Success)
            {
                var message = request.LogMessage;
                Log.Warning("{Message}-{UserId}-{Guid}-{ErrorType}-{ExceptionType}-{ExceptionMessage}-{Error_Message}", message, GlobalApplicationProperty.UserID, request.ServiceLogID, "Response", request.ExceptionType, request.ExceptionMessage);
            }
            await dbLogService!.UpdateRequestAsync(request);
        }
        #endregion
    }
}
