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
    public class CRLessonPodSummaryReportRepository : GenericRepository, ICRLessonPodSummaryReportRepository
    {
        #region CRLessonPodSummaryReportAsync
        public async Task<CRLessonPodSummaryReportResponse> CRLessonPodSummaryReportAsync(int LoginID, int LessonUnitDistID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            CRLessonPodSummaryReportResponse response = new();
            CRLessonPodSummaryReportData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("LoginID", LoginID, DbType.Int32);
                parameters.Add("LessonUnitDistID", LessonUnitDistID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaLessonpods + "GetCRLessonPodSummaryReport", parameters, commandType: CommandType.StoredProcedure));

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
                var listActivity = dbResponse!.Read<CRLessonPodSummaryReportActivityData>().ToList();
                var studentSummary = dbResponse!.Read<CRLessonPodSummaryReportStudentSummaryQueryData>().ToList();
                var podSummary = dbResponse!.Read<CRLessonPodSummaryReportSummaryData>().ToList();
                #endregion

                #region Declaration                
                List<string> listStudent = new();
                List<string> listSpentTime = new();
                List<float> listScore = new();
                List<float> listTimelyCompletion = new();
                List<int> listSpentTimeSeconds = new();
                List<string> listSpentTimeStudents = new();
                List<string> listScoreStudents = new();
                List<string> listTimelyCompletionStudents = new();
                #endregion

                #region Set response data structure
                if (studentSummary.Any())
                {
                    foreach (var studentData in studentSummary)
                    {
                        listStudent.Add(studentData.FullName);
                        listSpentTime.Add(studentData.AverageTimeSpent);
                        listScore.Add(studentData.AverageScore);
                        listTimelyCompletion.Add(studentData.AverageTimelyCompletion);
                        listSpentTimeSeconds.Add(studentData.AverageTimeTaken);
                        listSpentTimeStudents.Add(studentData.FullName + " : " + studentData.AverageTimeSpent);
                        listScoreStudents.Add(studentData.FullName + " : " + studentData.AverageScore + "%");
                        listTimelyCompletionStudents.Add(studentData.FullName + " : " + studentData.AverageTimelyCompletion + "%");
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
                if (podSummary.Any())
                {
                    var podData = podSummary.First();

                    responseData.LessonUnitDistID = podData.LessonUnitDistID;
                    responseData.AverageScore = podData.AverageScore;
                    responseData.AverageTimeSpent = podData.AverageTimeSpent;
                    responseData.AverageTimelyCompletion = podData.AverageTimelyCompletion;
                }

                responseData.Activity = listActivity;
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
