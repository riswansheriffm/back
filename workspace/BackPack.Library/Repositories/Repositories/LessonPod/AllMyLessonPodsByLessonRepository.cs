using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Library.Responses.User;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class AllMyLessonPodsByLessonRepository : GenericRepository, IAllMyLessonPodsByLessonRepository
    {
        #region AllMyLessonPodsByLessonAsync
        public async Task<AllMyLessonPodsByLessonResponse> AllMyLessonPodsByLessonAsync(int LessonID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllMyLessonPodsByLessonResponse response = new();
            AllMyLessonPodsByLessonData responseResult = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_id", LessonID, DbType.Int32);
                parameters.Add("ip_author_id", LoginID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_public_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_course_capsule_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllMyLessonPodsByLesson, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllMyLessonPodData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outPublicRecordSet = parameters.Get<string>("op_public_record_set");
                string publicRecordSetQuery = GlobalHelper.StringToString(outPublicRecordSet);
                var publicRecordSetData = await dbConnection.QueryAsync<AllMyLessonPodData>(publicRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outCourseCapsuleRecordSet = parameters.Get<string>("op_course_capsule_record_set");
                string courseCapsuleRecordSetQuery = GlobalHelper.StringToString(outCourseCapsuleRecordSet);
                var courseCapsuleRecordSetData = await dbConnection.QueryAsync<AllMyLessonPodCapsuleData>(courseCapsuleRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listMyLesson = recordSetData.ToList();
                var listSharedLesson = publicRecordSetData.ToList();
                var listCapsuleLesson = courseCapsuleRecordSetData.ToList();
                #endregion


                #region Response data
                responseResult.MyLessons = listMyLesson;
                responseResult.SharedLessons = listSharedLesson;
                responseResult.CapsuleLessons = listCapsuleLesson;
                AllMyLessonPodsByLessonResult responseData = new AllMyLessonPodsByLessonResult
                {
                    GetAllMyLessonPodsByLessonResult = responseResult
                };
                #endregion

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
