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
    public class LessonPodSummaryReportRepository : GenericRepository, ILessonPodSummaryReportRepository
    {
        #region LessonPodSummaryAnalyticsReportAsync
        public async Task<LessonPodSummaryAnalyticsReportResponse> LessonPodSummaryAnalyticsReportAsync(int LessonUnitDistID, int StudentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            LessonPodSummaryAnalyticsReportResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("LessonUnitDistID", LessonUnitDistID, DbType.Int32);
                parameters.Add("StudentID", StudentID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaLessonpods + "GetLessonPodSummaryAnalyticsReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response                
                var listData = dbResponse.Read<LessonPodSummaryAnalyticsReportQueryResponseData>().ToList();
                var testAverage = dbResponse!.Read<AnalyticsReportSummaryAverageData>().First();
                var practiceAverage = dbResponse!.Read<AnalyticsReportSummaryAverageData>().First();
                var lesson = dbResponse!.Read<LessonPodSummaryAnalyticsReportData>().ToList();
                #endregion

                #region Declaration
                LessonPodSummaryAnalyticsReportData responseData = new();
                List<LessonPodSummaryAnalyticsReportActivityData> listActivity = new();
                List<int> listStudentTimeTaken = new();
                List<int> listClassTimeTaken = new();
                List<int> listPracticeStudentTimeTaken = new();
                List<int> listPracticeClassTimeTaken = new();
                List<string> listStudentSpentTime = new();
                List<string> listClassSpentTime = new();
                List<string> listStudentAverageScore = new();
                List<string> listClassAverageScore = new();
                List<string> listStudentScore = new();
                List<string> listClassScore = new();
                List<string> listStudentAverageTimelyCompletion = new();
                List<string> listClassAverageTimelyCompletion = new();
                List<string> listStudentTimelyCompletion = new();
                List<string> listClassTimelyCompletion = new();
                List<string> listLessonPodActivityName = new();
                List<string> listPracticeStudentSpentTime = new();
                List<string> listPracticeClassSpentTime = new();
                List<string> listPracticeStudentTimelyCompletion = new();
                List<string> listPracticeClassTimelyCompletion = new();
                List<string> listPracticeStudentAverageTimelyCompletion = new();
                List<string> listPracticeClassAverageTimelyCompletion = new();
                List<string> listPracticeStudentAverageScore = new();
                List<string> listPracticeClassAverageScore = new();
                List<string> listPracticeStudentScore = new();
                List<string> listPracticeClassScore = new();
                #endregion

                #region Set response data structure                
                foreach (var data in listData)
                {
                    LessonPodSummaryAnalyticsReportActivityData activityData = new();
                    activityData.ContentID = data.ContentID;
                    activityData.ContentName = data.ContentName;
                    activityData.ActivityType = data.ActivityType;
                    activityData.AppName = data.AppName;
                    activityData.StudentScore = data.TestStudentScore;
                    activityData.ClassScore = data.TestClassAverageScore;
                    activityData.StudentTimeSpent = data.TestStudentTimeSpent;
                    activityData.ClassTimeSpent = data.TestClassTimeSpent;
                    activityData.StudentTimelyCompletion = data.TestStudentTimelyCompletion;
                    activityData.ClassTimelyCompletion = data.TestClassAverageTimelyCompletion;
                    activityData.ControlCount = data.ControlCount;
                    activityData.ActivityStatus = data.Status;
                    activityData.PracticeStudentScore = data.PrStudentMasteryIndex;
                    activityData.PracticeClassScore = data.PrClassAvgMasteryIndex;
                    activityData.PracticeStudentTimeSpent = data.PrStudentTimeSpent;
                    activityData.PracticeClassTimeSpent = data.PrClassTimeSpent;
                    activityData.PracticeStudentTimelyCompletion = data.PrStudentTimelyCompletion;
                    activityData.PracticeClassTimelyCompletion = data.PrClassAvgTimelyCompletion;

                    listActivity.Add(activityData);
                    listStudentTimeTaken.Add(data.TestStudentTimeTaken);
                    listClassTimeTaken.Add(data.TestClassTimeTaken);
                    listPracticeStudentTimeTaken.Add(data.StudentCumulativeTimeTaken);
                    listPracticeClassTimeTaken.Add(data.ClassAverageCumulativeTimeTaken);
                    listStudentSpentTime.Add(data.ContentName + ": " + data.TestStudentTimeSpent);
                    listClassSpentTime.Add(data.ContentName + ": " + data.TestClassTimeSpent);
                    listStudentAverageScore.Add(data.TestStudentScore.ToString());
                    listClassAverageScore.Add(data.TestClassAverageScore.ToString());
                    listStudentScore.Add(data.ContentName + ": " + data.TestStudentScore.ToString());
                    listClassScore.Add(data.ContentName + ": " + data.TestClassAverageScore.ToString());
                    listStudentAverageTimelyCompletion.Add(data.TestStudentTimelyCompletion.ToString());
                    listClassAverageTimelyCompletion.Add(data.TestClassAverageTimelyCompletion.ToString());
                    listStudentTimelyCompletion.Add(data.ContentName + ": " + data.TestStudentTimelyCompletion.ToString());
                    listClassTimelyCompletion.Add(data.ContentName + ": " + data.TestClassAverageTimelyCompletion.ToString());
                    listLessonPodActivityName.Add(data.ContentName!);
                    listPracticeStudentSpentTime.Add(data.ContentName + ": " + data.PrStudentTimeSpent);
                    listPracticeClassSpentTime.Add(data.ContentName + ": " + data.PrClassTimeSpent);
                    listPracticeStudentAverageScore.Add(data.PrStudentMasteryIndex.ToString());
                    listPracticeClassAverageScore.Add(data.PrClassAvgMasteryIndex.ToString());
                    listPracticeStudentScore.Add(data.ContentName + ": " + data.PrStudentMasteryIndex.ToString());
                    listPracticeClassScore.Add(data.ContentName + ": " + data.PrClassAvgMasteryIndex.ToString());
                    listPracticeStudentAverageTimelyCompletion.Add(data.PrStudentTimelyCompletion.ToString());
                    listPracticeClassAverageTimelyCompletion.Add(data.PrClassAvgTimelyCompletion.ToString());
                    listPracticeStudentTimelyCompletion.Add(data.ContentName + ": " + data.PrStudentTimelyCompletion.ToString());
                    listPracticeClassTimelyCompletion.Add(data.ContentName + ": " + data.PrClassAvgTimelyCompletion.ToString());
                }
                responseData.StudentTimeTaken = listStudentTimeTaken;
                responseData.ClassTimeTaken = listClassTimeTaken;
                responseData.PracticeStudentTimeTaken = listPracticeStudentTimeTaken;
                responseData.PracticeClassTimeTaken = listPracticeClassTimeTaken;
                responseData.StudentSpentTime = listStudentSpentTime;
                responseData.ClassSpentTime = listClassSpentTime;
                responseData.StudentAverageScore = listStudentAverageScore;
                responseData.ClassAverageScore = listClassAverageScore;
                responseData.StudentScore = listStudentScore;
                responseData.ClassScore = listClassScore;
                responseData.StudentAverageTimelyCompletion = listStudentAverageTimelyCompletion;
                responseData.ClassAverageTimelyCompletion = listClassAverageTimelyCompletion;
                responseData.StudentTimelyCompletion = listStudentTimelyCompletion;
                responseData.ClassTimelyCompletion = listClassTimelyCompletion;
                responseData.LessonPodActivityName = listLessonPodActivityName;
                responseData.PracticeStudentSpentTime = listPracticeStudentSpentTime;
                responseData.PracticeClassSpentTime = listPracticeClassSpentTime;
                responseData.PracticeStudentTimelyCompletion = listPracticeStudentTimelyCompletion;
                responseData.PracticeClassTimelyCompletion = listPracticeClassTimelyCompletion;
                responseData.PracticeStudentAverageTimelyCompletion = listPracticeStudentAverageTimelyCompletion;
                responseData.PracticeClassAverageTimelyCompletion = listPracticeClassAverageTimelyCompletion;
                responseData.PracticeStudentAverageScore = listPracticeStudentAverageScore;
                responseData.PracticeClassAverageScore = listPracticeClassAverageScore;
                responseData.PracticeStudentScore = listPracticeStudentScore;
                responseData.PracticeClassScore = listPracticeClassScore;
                responseData.StudentLessonPodAverage = testAverage;
                responseData.PracticeStudentLessonPodAverage = practiceAverage;
                if (lesson.Any())
                {
                    var lessonData = lesson.First();
                    responseData.LessonName = lessonData.LessonName;
                    responseData.LessonDesc = lessonData.LessonDesc;
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
