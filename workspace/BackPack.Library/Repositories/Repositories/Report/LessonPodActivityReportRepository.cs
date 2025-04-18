using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class LessonPodActivityReportRepository : GenericRepository, ILessonPodActivityReportRepository
    {
        #region LessonPodActivityAnalyticsReportAsync
        public async Task<LessonPodActivityAnalyticsReportResponse> LessonPodActivityAnalyticsReportAsync(int ContentID, int StudentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            LessonPodActivityAnalyticsReportResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("StudentID", StudentID, DbType.Int32);
                parameters.Add("ContentID", ContentID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaLessonpods + "GetLessonPodActivityAnalyticsReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response
                var content = dbResponse!.Read<LessonPodActivityAnalyticsReportQueryData>().ToList();
                var login = dbResponse!.Read<LessonPodActivityAnalyticsReportQueryData>().ToList();
                var activity = dbResponse!.Read<LessonPodActivityAnalyticsReportQueryData>().ToList();
                #endregion

                #region Declaration
                LessonPodActivityAnalyticsReportData responseData = new();
                #endregion

                #region Set response data structure
                if (content.Any())
                {
                    var contentData = content.First();

                    responseData.AppName = contentData.AppName;
                    responseData.FollowTheFlow = contentData.FollowTheFlow;
                    responseData.SearchName = contentData.SearchName;
                    responseData.SearchTag = contentData.SearchTag;
                }
                if (login.Any())
                {
                    var loginData = login.First();

                    responseData.LoginName = loginData.LoginName;
                    responseData.FullName = loginData.FullName;
                }
                if (activity.Any())
                {
                    var listControlData = dbResponse.Read<LessonPodActivityAnalyticsReportControlData>().ToList();
                    var activityData = activity.First();

                    responseData.ContentID = activityData.ContentID;
                    responseData.ClassTimeTaken = activityData.ClassTimeTaken;
                    responseData.ClassTimeSpent = activityData.ClassTimeSpent;
                    responseData.ClassScore = activityData.ClassScore;
                    responseData.ClassTimelyCompletion = activityData.ClassTimelyCompletion;
                    responseData.StudentTimeSpent = activityData.StudentTimeSpent;
                    responseData.StudentTimeTaken = activityData.StudentTimeTaken;
                    responseData.StudentScore = activityData.StudentScore;
                    responseData.StudentTimelyCompletion = activityData.StudentTimelyCompletion;
                    responseData.ActivityControls = listControlData;
                }
                #endregion

                #region Response
                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                return response;
            }
            catch (NpgsqlException ex)
            {
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeSqlException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
