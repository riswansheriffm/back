using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class SyncLessonPodRepository : GenericRepository, ISyncLessonPodRepository
    {
        #region SyncCourseDownloadAsync
        public async Task<SyncCourseDownloadResponse> SyncCourseDownloadAsync(int StudentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            SyncCourseDownloadResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetSyncCourseDownload, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SyncCourseDownloadData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response
                response.Data = recordSetData.ToList();
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

        #region SyncLessonUnitDownloadAsync
        public async Task<SyncLessonUnitDownloadResponse> SyncLessonUnitDownloadAsync(int StudentID, int LessonUnitDistID, int PreviousLessonUnitDistID)
        {
            SyncLessonUnitDownloadResponse response = new();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_previous_lesson_unit_dist_id", PreviousLessonUnitDistID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_distribution_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetSyncLessonUnitDownload, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SyncLessonUnitDownloadQueryResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outDistributionRecordSet = parameters.Get<string>("op_distribution_record_set");
                string distributionRecordSetQuery = GlobalHelper.StringToString(outDistributionRecordSet);
                var distributionRecordSetData = await dbConnection.QueryAsync<SyncLessonUnitDownloadData>(distributionRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Set response structure
                var listActivity = recordSetData.ToList();
                var lessonPod = distributionRecordSetData.FirstOrDefault();
                List<SyncLessonUnitDownloadActivityData> listActivityData = new();

                foreach (var activity in listActivity)
                {
                    SyncLessonUnitDownloadActivityData activityData = new()
                    {
                        ContentID = activity.ContentID,
                        Completed = activity.Completed,
                        IsReadonly = activity.IsReadonly,
                        ActivityJson = activity.ActivityJson,
                        WorkJson = activity.WorkJson,
                        InkJson = activity.InkJson,
                        ActivityType = activity.ActivityType,
                        ContentMode = activity.ContentMode,
                        TargetDateOfCompletion = activity.TargetDateOfCompletion,
                        TargetTimeOfCompletion = activity.TargetTimeOfCompletion,
                        SpentTime = activity.SpentTime,
                        ContentName = activity.ContentName,
                        AppName = activity.AppName,
                        TargetCompletionDateTime = activity.TargetCompletionDateTime,
                        Status = activity.Status
                    };

                    List<BackpackActivityForStudentFeedback> listFeedbackData = new();
                    BackpackActivityForStudentFeedback feedbackData = new()
                    {
                        ReworkCount = activity.ReworkCount,
                        Grade = activity.Grade!,
                        Feedback = activity.Feedback!,
                        FeedbackDate = activity.FeedbackDate!,
                        Remarks = activity.Remarks!,
                        ModifiedDate = activity.ModifiedDate!
                    };

                    listFeedbackData.Add(feedbackData);
                    activityData.Feedback = listFeedbackData;
                    listActivityData.Add(activityData);
                }

                lessonPod!.Activities = listActivityData;
                #endregion

                #region Response
                response.Data = lessonPod;
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

        #region SyncCourseLessonUnitDownloadAsync
        public async Task<SyncCourseLessonUnitDownloadResponse> SyncCourseLessonUnitDownloadAsync(int StudentID, string CourseIDs)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            SyncCourseLessonUnitDownloadResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_course_ids", CourseIDs, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetSyncCourseLessonUnitDownload, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SyncCourseLessonUnitDownloadData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response
                response.Data = recordSetData.ToList();
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
