using BackPack.Library.Helpers.LessonPod;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class AllActivitiesByLessonPodRepository : GenericRepository, IAllActivitiesByLessonPodRepository
    {
        #region AllActivitiesByLessonPodAsync
        public async Task<AllActivitiesByLessonPodResponse> AllActivitiesByLessonPodAsync(int LessonUnitID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            AllActivitiesByLessonPodResponse response = new();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetLessonUnit, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllActivitiesByLessonPodQueryData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response
                if (recordSetData.Any())
                {
                    var lessonPodData = recordSetData.First();
                    AllActivitiesByLessonPodResult responseData = new AllActivitiesByLessonPodResult
                    {
                        GetAllActivitiesByLessonUnitResult = LessonPodHelper.LessonPodActivityList(lessonPodData.LessonJson)
                    };
                    response.Data = responseData;
                }
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeSqlException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
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
