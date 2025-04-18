using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Library.Responses.Master.Group;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class GetLessonRepository : GenericRepository, IGetLessonRepository
    {
        #region GetLessonAsync        
        public async Task<LessonResponse> GetLessonAsync(int LessonID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_id", LessonID, DbType.Int32);
                parameters.Add("op_lesson_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_tag_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetLesson, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_lesson_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetLessonResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outTagRecordSet = parameters.Get<string>("op_tag_record_set");
                string tagRecordSetQuery = GlobalHelper.StringToString(outTagRecordSet);
                var tagRecordSetData = await dbConnection.QueryAsync<Tags>(tagRecordSetQuery, sqlTransaction, commandType: CommandType.Text);


                var lesson = recordSetData;
                var details = tagRecordSetData;

                var responseData = new LessonResponseData
                {
                    GetLessonResult = new GetLessonResult
                    {
                        LessonID = lesson?.FirstOrDefault()?.LessonID ?? 0,
                        SubjectID = lesson?.FirstOrDefault()?.SubjectID ?? 0,
                        LessonName = lesson?.FirstOrDefault()?.LessonName,
                        LessonDesc = lesson?.FirstOrDefault()?.LessonDesc,
                        ImageURL = lesson?.FirstOrDefault()?.ImageURL,
                        ChapterID = lesson?.FirstOrDefault()?.ChapterID ?? 0,
                        ChapterName = lesson?.FirstOrDefault()?.ChapterName,
                        Tags = details.ToList()
                    }
                };

                #region Response
                LessonResponse response = new()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                sqlTransaction.Commit();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new LessonResponse
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
                return new LessonResponse
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
