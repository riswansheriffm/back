using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class EnableCourseCapsuleRepository : GenericRepository, IEnableCourseCapsuleRepository
    {
        #region EnableCourseCapsuleAsync        
        public async Task<BaseResponse> EnableCourseCapsuleAsync(DeleteCourseCapsuleRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", request.LoginID, DbType.Int32);
                parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.EnableCourseCapsule, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_return_id", DbType.Int64, direction: ParameterDirection.Output);
                #endregion

                await dbConnection!.QueryMultipleAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.EnableCourseCapsule, parameters, commandType: CommandType.StoredProcedure);
                int ReturnID = parameters.Get<int>("op_return_id");

                if (ReturnID == 0)
                {
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = CourseCapsuleMessage.EnableCourseCapsule
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                return new BaseResponse()
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = CommonMessage.ExceptionMessage,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.Message + " : " + ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = CommonMessage.ExceptionMessage,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.Message + " : " + ex.StackTrace
                };
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}    
