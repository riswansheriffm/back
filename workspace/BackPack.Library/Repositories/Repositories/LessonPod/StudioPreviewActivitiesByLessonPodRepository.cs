
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Globalization;
using System.Text.Json;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class StudioPreviewActivitiesByLessonPodRepository : GenericRepository, IStudioPreviewActivitiesByLessonPodRepository
    {
        #region GetLPStudioPreviewActivitiesByLessonPodAsync
        public async Task<StudioPreviewActivitiesByLessonPodResponse> GetLPStudioPreviewActivitiesByLessonPodAsync(int LoginID, int LessonUnitID, string PreviewMode)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            StudioPreviewActivitiesByLessonPodResponse response = new();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
                parameters.Add("ip_preview_mode", PreviewMode, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_lessonpod_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetLpStudioPreviewActivitiesByLessonPod, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<StudioPreviewActivitiesByLessonPodActivityResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outLessonpodRecordSet = parameters.Get<string>("op_lessonpod_record_set");
                string lessonpodRecordSetQuery = GlobalHelper.StringToString(outLessonpodRecordSet);
                var lessonpodRecordSetData = await dbConnection.QueryAsync<StudioPreviewActivitiesByLessonPodResult>(lessonpodRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                if (lessonpodRecordSetData.Any())
                {
                    foreach (var item in recordSetData)
                    {
                        if (item.ContainedViewData != "")
                        {
                            item.ContainedView = JsonSerializer.Deserialize<List<object>>(item.ContainedViewData.ToString())!;
                        }
                    }

                    lessonpodRecordSetData.FirstOrDefault()!.Activities = recordSetData.ToList();
                }

                var responseData = new StudioPreviewActivitiesByLessonPodData
                {
                    GetLPStudioPreviewActivitiesByLessonPodResult = lessonpodRecordSetData.FirstOrDefault()!
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
