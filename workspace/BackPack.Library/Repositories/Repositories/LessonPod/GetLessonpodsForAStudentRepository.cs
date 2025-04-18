
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class GetLessonpodsForAStudentRepository : GenericRepository, IGetLessonpodsForAStudentRepository
    {
        #region GetLessonPodsForAStudentAsync
        public async Task<LessonpodsForAStudentResponse> GetLessonpodsForAStudentAsync(int studentId, int courseId, int lessonId, int chapterId, int parentId)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            LessonpodsForAStudentResponse response = new();
            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", studentId, DbType.Int32);
                parameters.Add("ip_lesson_id", lessonId, DbType.Int32);
                parameters.Add("ip_parent_id", parentId, DbType.Int32);
                parameters.Add("ip_chapter_id", chapterId, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetBpCompletedLessonUnitsByLesson, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CompletedLessonPodsByLessonData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Set parameters            
                var pendingParameters = new DynamicParameters();
                pendingParameters.Add("ip_student_id", studentId, DbType.Int32);
                pendingParameters.Add("ip_course_id", courseId, DbType.Int32);
                pendingParameters.Add("ip_parent_id", parentId, DbType.Int32);
                pendingParameters.Add("ip_lesson_id", lessonId, DbType.Int32);
                pendingParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetPendingLessonUnitsForAStudent, pendingParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outPendingRecordSet = pendingParameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetPendingQuery = GlobalHelper.StringToString(outPendingRecordSet);
                var recordSetPendingData = await dbConnection.QueryAsync<PendingLessonUnitsForAStudentData>(recordSetPendingQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new LessonpodsForAStudentResult
                {
                    Completed = recordSetData.ToList(),
                    Pending = recordSetPendingData.ToList()
                };

                #region Response
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
