using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class UpdateLessonInLessonPodRepository : GenericRepository, IUpdateLessonInLessonPodRepository
    {
        #region UpdateLessonInLessonPodAsync
        public async Task<BaseResponse> UpdateLessonInLessonPodAsync(UpdateLessonInLessonPodRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", request.LoginID, DbType.Int32);
                parameters.Add("ip_lesson_id", request.LessonID, DbType.Int32);
                parameters.Add("ip_lesson_unit_id", request.LessonUnitID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_activity_desc", LessonPodMessage.UpdateLessonInLessonPod, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_return_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion
                 
                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.UpdateLessonForAContent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #region Response
                int ReturnStatus = parameters.Get<int>("op_return_status");

                if (ReturnStatus == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UpdateLessonInLessonPodSuccess
                    };
                }

                if (ReturnStatus == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.UpdateLessonInLessonPodFail,
                        ExceptionMessage = LessonPodMessage.LessonPodNotExist
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.UpdateLessonInLessonPodFail
                    };
                }
                #endregion
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
