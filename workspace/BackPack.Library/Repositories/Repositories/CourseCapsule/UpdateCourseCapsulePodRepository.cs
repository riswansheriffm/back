using BackPack.Dependency.Library.Messages;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class UpdateCourseCapsulePodRepository : GenericRepository, IUpdateCourseCapsulePodRepository
    {
        #region UpdateCourseCapsulePodAsync
        public async Task<BaseResponse> UpdateCourseCapsulePodAsync(UpdateCourseCapsulePodRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();

            try
            {
                int podCount = 0;
                foreach (var pod in request.Pods!)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_course_capsule_folder_id", request.CourseCapsuleFolderID, DbType.Int32);
                    parameters.Add("ip_course_capsule_lessonpod_id", pod.CourseCapsuleLessonPodID, DbType.Int32);
                    parameters.Add("ip_display_order", pod.DisplayOrder, DbType.Int32);
                    parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    parameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsuleFolderReorder, DbType.String);
                    parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    parameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.UpdateCourseCapsulePodReorder, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    int statudId = parameters.Get<int>("op_return_status");
                    if (statudId == 1)
                    {
                        podCount++;
                    }
                    else
                    {
                        break;
                    }                    
                }

                if (podCount > 0 && request.Pods.Count == podCount)
                {
                    await sqlTransaction.CommitAsync();
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = CourseCapsuleMessage.UpdateCourseCapsulePod;
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.ErrorID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                }

                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
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
                await sqlTransaction.RollbackAsync();
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
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
