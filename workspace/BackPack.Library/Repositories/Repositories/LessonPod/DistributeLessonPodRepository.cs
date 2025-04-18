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
    public class DistributeLessonPodRepository : GenericRepository, IDistributeLessonPodRepository
    {
        #region DistributeLessonPodAsync
        public async Task<DistributeLessonPodResponse> DistributeLessonPodAsync(int LessonUnitID, string LessonType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            DistributeLessonPodResponse response = new();
            DistributeLessonPodData responseData = new();

            try
            {
                #region Lessonpod
                if (LessonType != "CapsuleLesson")
                {
                    #region Set parameters            
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
                    parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetLessonUnit, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    var outRecordSet = parameters.Get<string>("op_record_set");
                    string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                    var recordSetData = await dbConnection.QueryAsync<DistributeLessonPodQueryResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                    var lessonPod = recordSetData.ToList();
                    if (lessonPod.Count > 0)
                    {
                        var lessonData = lessonPod.First();

                        responseData.LessonUnitID = lessonData.LessonUnitID;
                        responseData.LessonName = lessonData.LessonName;
                        responseData.LessonDesc = lessonData.LessonDesc;
                        responseData.Slides = LessonPodHelper.LessonPodSlideList(lessonData.LessonJson!);
                    }                    
                }
                #endregion                

                #region Course capsule lessonpod
                if (LessonType == "CapsuleLesson")
                {
                    #region Set parameters            
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_publish_course_capsule_lessonpod_id", LessonUnitID, DbType.Int32);
                    parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    parameters.Add("op_slide_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetPublishedIlCourseCapsuleLessonpod, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    var outRecordSet = parameters.Get<string>("op_record_set");
                    string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                    var recordSetData = await dbConnection.QueryAsync<DistributeLessonPodQueryResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                    var outSlideRecordSet = parameters.Get<string>("op_slide_record_set");
                    string slideRecordSetQuery = GlobalHelper.StringToString(outSlideRecordSet);
                    var slideRecordSetData = await dbConnection.QueryAsync<LessonPodSlideListResponse>(slideRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                    var lessonPod = recordSetData.ToList();
                    if (lessonPod.Count > 0)
                    {
                        var lessonData = lessonPod.First();
                        var slide = slideRecordSetData.ToList();

                        responseData.LessonUnitID = lessonData.LessonUnitID;
                        responseData.LessonName = lessonData.LessonName;
                        responseData.LessonDesc = lessonData.LessonDesc;
                        responseData.Slides = slide;
                    }
                }
                DistributeLessonPodDataResult responseResult = new()
                {
                    GetDistributeLessonUnitResult = responseData,
                };
                #endregion

                #region Response
                response.Data = responseResult;
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
