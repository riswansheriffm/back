
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class StudioPreviewActivityBySlideRepository : GenericRepository, IStudioPreviewActivityBySlideRepository
    {
        #region GetLPStudioPreviewActivityBySlideAsync
        public async Task<StudioPreviewActivityBySlideResponse> GetLPStudioPreviewActivityBySlideAsync(int LoginID, int LessonUnitID, string SlideID, string PreviewMode)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            StudioPreviewActivityBySlideResponse response = new();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
                parameters.Add("ip_slide_id", SlideID, DbType.String);
                parameters.Add("ip_preview_mode", PreviewMode, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_other_activity_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetLpStudioPreviewActivityBySlide, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<LPStudioPreviewActivityBySlideActivity>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outOtherActivityRecordSet = parameters.Get<string>("op_other_activity_record_set");
                string otherActivityRecordSetQuery = GlobalHelper.StringToString(outOtherActivityRecordSet);
                var otherActivityRecordSetData = await dbConnection.QueryAsync<LPStudioPreviewActivityBySlideData>(otherActivityRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                LPStudioPreviewActivityBySlideActivity slideActivity = new();
                if (recordSetData.Any())
                {
                    slideActivity = recordSetData.FirstOrDefault()!;
                    slideActivity.OtherActivities = otherActivityRecordSetData.ToList();
                }

                var responseData = new LPStudioPreviewActivityBySlideResult
                {
                    GetLPStudioPreviewActivityBySlideResult = slideActivity
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
