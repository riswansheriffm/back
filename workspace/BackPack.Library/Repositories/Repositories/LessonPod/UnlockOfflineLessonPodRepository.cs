using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class UnlockOfflineLessonPodRepository : GenericRepository, IUnlockOfflineLessonPodRepository
    {
        #region UnlockOfflineLessonUnitByCourseIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByCourseIDAsync(UnlockOfflineLessonPodRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("CourseID", request.CourseID, DbType.Int64);
                parameters.Add("StudentID", request.LoginID, DbType.Int64);
                #endregion

                var result = await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + "UnlockOfflineLessonUnitByCourseID", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #region Response
                if (result > 0)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UnlockOfflineSuccess
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UnlockOfflineNoRecord
                    };
                }
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            finally
            {
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();                
            }
        }
        #endregion

        #region UnlockOfflineLessonUnitByDistIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByDistIDAsync(UnlockOfflineLessonPodDistRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_student_id", request.LoginID, DbType.Int32);
                parameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.UnlockOfflineLessonUnitByOfflineLessonUnitDistId, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var result = parameters.Get<int>("op_status_id");

                #region Response                
                if (result == 1)
                {
                    await sqlTransaction.CommitAsync();
                }
                if (result == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UnlockOfflineNoRecord
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
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
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            finally
            {
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();                
            }
        }
        #endregion

        #region UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync
        public async Task<BaseResponse> UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(UnlockOfflineLessonPodDistRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int64);
                parameters.Add("ip_student_id", request.LoginID, DbType.Int64);
                parameters.Add("OutSop_status_idtatusID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.UnlockOfflineLessonUnitByOfflineLessonUnitDistId, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var result = parameters.Get<int>("op_status_id");

                #region Response                
                if (result == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UnlockOfflineSuccess
                    };
                }
                if (result == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.UnlockOfflineNoRecord
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
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
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
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
