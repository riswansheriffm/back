using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Services.Interfaces.DBLog;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers
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
            
            Log.Warning("{Message}-{UserId}-{Guid}-{ErrorType}-{ErrorMessage}", message, GlobalApplicationProperty.UserID, guid, "Validation", validation);
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
                Log.Warning("{Message}-{UserId}-{Guid}-{ErrorType}-{ExceptionType}-{ExceptionMessage}-{ErrorMessage}", message, GlobalApplicationProperty.UserID, request.ServiceLogID, "Response", request.ExceptionType, request.ExceptionMessage);
            }
            await dbLogService!.UpdateRequestAsync(request);
        }
        #endregion

        #region IsValidDomainUser
        public static bool IsValidDomainUser(int domainId)
        {
            if (GlobalApplicationProperty.DomainID == domainId)
            {
                return true;
            }
            else
            {
                var message = CommonMessage.InvalidDomainMessage;
                Log.Warning("{Message}-{UserId}-{ErrorMessage}", message, GlobalApplicationProperty.UserID, "Invalid DomainUser");
                return false;
            }
        }
        #endregion

        #region IsValidUserRole
        public static bool IsValidUserRole(string role)
        {
            if ((role == "DistrictAdmin" &&  (GlobalApplicationProperty.UserRole == "CurriculumAdmin" || GlobalApplicationProperty.UserRole == "SchoolAdmin")) ||
                (role == "CurriculumAdmin" && GlobalApplicationProperty.UserRole == "SchoolAdmin"))
            {
                var message = CommonMessage.InvalidDomainMessage;
                Log.Warning("{Message}-{UserId}-{ErrorMessage}", message, GlobalApplicationProperty.UserID, "Invalid DomainUser");
                return false;                
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region ReturnBadRequestResponse
        public static BadRequestResponse ReturnBadRequestResponse()
        {
            return new BadRequestResponse
            {
                MessageID = 0,
                Success = false,
                StatusCode = 400,
                StatusMessage = CommonMessage.InvalidDomainMessage
            };
        }
        #endregion
    }
}
