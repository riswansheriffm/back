using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Library.Responses.Activity;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class LessonPodDetailsRepository : GenericRepository, ILessonPodDetailsRepository
    {
        #region LessonPodDetailsForAStudentAsync
        public async Task<LessonPodDetailsForAStudentResponse> LessonPodDetailsForAStudentAsync(int StudentID, int LessonUnitDistID, int ParentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            LessonPodDetailsForAStudentResponse response = new();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_parent_id", ParentID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetLessonUnitDetailsForAStudent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<LessonPodDetailsForAStudentData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response
                LessonPodDetailsForAStudentResult responseData = new LessonPodDetailsForAStudentResult
                {
                    GetLessonUnitDetailsForAStudentResult = recordSetData.ToList()
                };

                response.Data = responseData;
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
