using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class RecallLessonPodDistributionRepository : GenericRepository, IRecallLessonPodDistributionRepository
    {
        #region RecallLessonPodDistributionAsync
        public async Task<BaseResponse> RecallLessonPodDistributionAsync(RecallLessonPodDistributionRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Audit log
                DistributionAuditLogResponse auditLogRequest = new()
                {
                    LessonUnitDistID = request.LessonUnitDistID,
                    UserID = request.LoginID,
                    DistributionTypeID = 3,
                    ActivityDescription = LessonPodMessage.LessonPodRecallDistributionAuditLog
                };

                await LessonPodDistributionCommon.SaveLessonPodDistributionAuditLogAsync(auditLogRequest, dbConnection, sqlTransaction);
                #endregion

                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
                parameters.Add("op_return_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.RecallLessonUnitDistribution, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #region Response
                int StatusID = parameters.Get<int>("op_return_status");

                if (StatusID == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.RecallLessonPodSuccess
                    };
                }

                if (StatusID == 0)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.RecallLessonPodFail
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
                        ExceptionType = CommonMessage.ExceptionTypeFail,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
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
