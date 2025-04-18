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
    public class UpdateCourseCapsuleActivityRepository : GenericRepository, IUpdateCourseCapsuleActivityRepository
    {
        #region UpdateCourseCapsuleActivityAsync          
        public async Task<BaseResponse> UpdateCourseCapsuleActivityAsync(UpdateCourseCapsuleActivityRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Update activity 
                int activityCount = 0;
                for (int index = 0; index < request.Activities?.Count; index++)
                {
                    UpdateCourseCapsuleActivityReorderList Class = request.Activities[index];

                    #region Set parameters                     
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_course_capsule_lessonpod_id", request.CourseCapsuleLessonPodID, DbType.Int32);
                    parameters.Add("ip_course_capsule_activity_id", Class.CourseCapsuleActivityID, DbType.Int32);
                    parameters.Add("ip_display_order", Class.DisplayOrder, DbType.Int32);
                    parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    parameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsuleFolderReorder, DbType.String);
                    parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    parameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.UpdateCourseCapsuleActivityReorder, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    int statudId = parameters.Get<int>("op_return_status");
                    if (statudId == 1)
                    {
                        activityCount++;
                    }
                    else
                    {
                        break;
                    }                    
                }
                #endregion

                if (activityCount > 0 && request.Activities!.Count == activityCount)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = CourseCapsuleMessage.UpdateCourseCapsuleActivities
                    };                    
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
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
