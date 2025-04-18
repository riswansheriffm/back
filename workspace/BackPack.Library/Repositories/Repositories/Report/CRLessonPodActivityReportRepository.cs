using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class CRLessonPodActivityReportRepository : GenericRepository, ICRLessonPodActivityReportRepository
    {
        #region CRLessonPodActivityReportAsync
        public async Task<CRLessonPodActivityReportResponse> CRLessonPodActivityReportAsync(int LoginID, int ContentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            CRLessonPodActivityReportResponse response = new();
            CRLessonPodActivityReportData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("LoginID", LoginID, DbType.Int32);
                parameters.Add("ContentID", ContentID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaLessonpods + "GetCRLessonPodActivitySummaryReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response
                var activityCount = dbResponse!.Read<DBQueryDataCountResponse>().ToList();

                if (activityCount[0].ActivityCount == 0)
                {
                    #region Response
                    response.Data = responseData;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.StatusMessage = CommonMessage.ReadMessage;
                    #endregion

                    return response;
                }
                var contentSummary = dbResponse!.Read<CRLessonPodActivityReportSummaryData>().ToList();
                var studentSummary = dbResponse!.Read<CRLessonPodActivityReportStudentSummaryQueryData>().ToList();
                var activitySummary = dbResponse!.Read<CRLessonPodActivityReportActivitySummaryQueryData>().ToList();
                var listControl = dbResponse!.Read<CRLessonPodActivityReportControlData>().ToList();
                #endregion

                #region Declaration                
                List<string> listStudent = new();
                List<string> listSpentTime = new();
                List<float> listScore = new();
                List<int> listTimelyCompletion = new();
                List<int> listSpentTimeSeconds = new();
                List<string> listSpentTimeStudents = new();
                List<string> listScoreStudents = new();
                List<string> listTimelyCompletionStudents = new();
                List<float> listStudentCount = new();
                List<string> listStudentSummary = new();
                #endregion

                #region Set response data structure
                if (contentSummary.Any())
                {
                    var contentSummaryData = contentSummary.First();

                    responseData.ContentID = contentSummaryData.ContentID;
                    responseData.AverageTimeSpent = contentSummaryData.AverageTimeSpent;
                    responseData.AverageScore = contentSummaryData.AverageScore;
                    responseData.AverageTimelyCompletion = contentSummaryData.AverageTimelyCompletion;
                }
                if (studentSummary.Any())
                {
                    foreach (var studentData in studentSummary)
                    {
                        listStudent.Add(studentData.FullName);
                        listSpentTime.Add(studentData.TimeSpent);
                        listScore.Add(studentData.ActivityScore);
                        listTimelyCompletion.Add(studentData.TimelyCompletion);
                        listSpentTimeSeconds.Add(studentData.TimeTaken);
                        listSpentTimeStudents.Add(studentData.FullName + " : " + studentData.TimeSpent);
                        listScoreStudents.Add(studentData.FullName + " : " + studentData.ActivityScore + "%");
                        listTimelyCompletionStudents.Add(studentData.FullName + " : " + studentData.TimelyCompletionBool);
                    }
                    responseData.Students = listStudent;
                    responseData.SpentTime = listSpentTime;
                    responseData.Score = listScore;
                    responseData.TimelyCompletion = listTimelyCompletion;
                    responseData.SpentTimeStudents = listSpentTimeStudents;
                    responseData.SpentTimeSeconds = listSpentTimeSeconds;
                    responseData.ScoreStudents = listScoreStudents;
                    responseData.TimelyCompletionStudents = listTimelyCompletionStudents;
                }
                if (activitySummary.Any())
                {
                    foreach (var activityData in activitySummary)
                    {
                        listStudentCount.Add(activityData.StudentCount);
                        listStudentSummary.Add(activityData.StudentSummary);
                    }
                    responseData.StudentSummary = listStudentSummary;
                    responseData.StudentCount = listStudentCount;
                }

                responseData.ActivityControls = listControl;
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
