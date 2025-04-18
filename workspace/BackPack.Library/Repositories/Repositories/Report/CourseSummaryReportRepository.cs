using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class CourseSummaryReportRepository : GenericRepository, ICourseSummaryReportRepository
    {
        #region CourseSummaryAnalyticsReportAsync
        public async Task<CourseSummaryAnalyticsReportResponse> CourseSummaryAnalyticsReportAsync(int DomainID, int CourseID, int StudentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            CourseSummaryAnalyticsReportResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("StudentID", StudentID, DbType.Int32);
                parameters.Add("DomainID", DomainID, DbType.Int32);
                parameters.Add("CourseID", CourseID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaStudents + "GetCourseSummaryAnalyticsReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response                
                var listData = dbResponse.Read<CourseSummaryAnalyticsReportQueryResponseData>().ToList();
                var testAverage = dbResponse!.Read<AnalyticsReportSummaryAverageData>().First();
                var practiceAverage = dbResponse!.Read<AnalyticsReportSummaryAverageData>().First();
                #endregion

                #region Declaration
                CourseSummaryAnalyticsReportData responseData = new();
                List<CourseSummaryAnalyticsReportLessonFolderData>? listLessonFolder = new();
                List<CourseSummaryAnalyticsReportLessonFolderLessonPodData>? listLessonPod = new();
                CourseSummaryAnalyticsReportLessonFolderData? lessonFolder = new();
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
                List<string> listLessonPodName = new();
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

                int LessonID = 0;
                int PreviousLessonID = 0;
                #endregion

                #region Set response data structure                
                foreach (var data in listData)
                {
                    if (LessonID != data.LessonID)
                    {
                        PreviousLessonID = LessonID;
                        if (PreviousLessonID != 0)
                        {
                            lessonFolder!.LessonPods = listLessonPod;
                            listLessonFolder.Add(lessonFolder);
                        }
                        LessonID = data.LessonID;
                        lessonFolder = new();
                        listLessonPod = new();
                        lessonFolder.LessonID = data.LessonID;
                        lessonFolder.LessonName = data.LessonName;
                        lessonFolder.LessonDesc = data.LessonDesc;
                    }

                    CourseSummaryAnalyticsReportLessonFolderLessonPodData lessonPod = new();
                    lessonPod.LessonUnitDistID = data.LessonUnitDistID;
                    lessonPod.LessonPodStatus = data.LessonPodStatus;
                    lessonPod.LessonPodName = data.LessonPodName;
                    lessonPod.LessonPodDesc = data.LessonPodDesc;
                    lessonPod.StudentTimeSpent = data.TestStudentTimeSpent;
                    lessonPod.StudentScore = data.TestStudentAverageScore;
                    lessonPod.StudentTimelyCompletion = data.TestStudentAverageTimelyCompletion;
                    lessonPod.ClassTimeSpent = data.TestClassTimeSpent;
                    lessonPod.ClassScore = data.TestClassAverageScore;
                    lessonPod.ClassTimelyCompletion = data.TestClassAverageTimelyCompletion;
                    lessonPod.LessonPodCompletion = data.LessonPodCompletion;
                    lessonPod.ReworkCount = data.ReworkCount;
                    lessonPod.PracticeStudentTimeSpent = data.PrStudentTimeSpent;
                    lessonPod.PracticeStudentMasteryIndex = data.PrStudentAvgMasteryIndex;
                    lessonPod.PracticeTimelyCompletion = data.PrStudentAvgTimelyCompletion;
                    lessonPod.PracticeStudentTotalTries = data.PrStudentTotalTries;
                    lessonPod.PracticeClassTimeSpent = data.PrClassTimeSpent;
                    lessonPod.PracticeClassMasteryIndex = data.PrClassAvgMasteryIndex;
                    lessonPod.PracticeClassCompletion = data.PrClassAvgTimelyCompletion;
                    lessonPod.PracticeClassTotalTries = data.PrClassTotalTries;
                    lessonPod.TestCount = data.TestCount;

                    listLessonPod!.Add(lessonPod);
                    listStudentTimeTaken.Add(data.TestStudentTimeTaken);
                    listClassTimeTaken.Add(data.TestClassTimeTaken);
                    listPracticeStudentTimeTaken.Add(data.StudentAverageCumulativeTimeTaken);
                    listPracticeClassTimeTaken.Add(data.ClassAverageCumulativeTimeTaken);
                    listStudentSpentTime.Add(data.LessonPodName + ": " + data.TestStudentTimeSpent);
                    listClassSpentTime.Add(data.LessonPodName + ": " + data.TestClassTimeSpent);
                    listStudentAverageScore.Add(data.TestStudentAverageScore!);
                    listClassAverageScore.Add(data.TestClassAverageScore!);
                    listStudentScore.Add(data.LessonPodName + ": " + data.TestStudentAverageScore);
                    listClassScore.Add(data.LessonPodName + ": " + data.TestClassAverageScore);
                    listStudentAverageTimelyCompletion.Add(data.TestStudentAverageTimelyCompletion!);
                    listClassAverageTimelyCompletion.Add(data.TestClassAverageTimelyCompletion!);
                    listStudentTimelyCompletion.Add(data.LessonPodName + ": " + data.TestStudentAverageTimelyCompletion);
                    listClassTimelyCompletion.Add(data.LessonPodName + ": " + data.TestClassAverageTimelyCompletion);
                    listLessonPodName.Add(data.LessonPodName!);
                    listPracticeStudentSpentTime.Add(data.PrStudentTimeSpent!);
                    listPracticeClassSpentTime.Add(data.PrClassTimeSpent!);
                    listPracticeStudentTimelyCompletion.Add(data.LessonPodName + ": " + data.PrStudentAvgTimelyCompletion);
                    listPracticeClassTimelyCompletion.Add(data.LessonPodName + ": " + data.PrClassAvgTimelyCompletion);
                    listPracticeStudentAverageTimelyCompletion.Add(data.PrStudentAvgTimelyCompletion!);
                    listPracticeClassAverageTimelyCompletion.Add(data.PrClassAvgTimelyCompletion!);
                    listPracticeStudentAverageScore.Add(data.PrStudentAvgMasteryIndex!);
                    listPracticeClassAverageScore.Add(data.PrClassAvgMasteryIndex!);
                    listPracticeStudentScore.Add(data.LessonPodName + ": " + data.PrStudentAvgMasteryIndex);
                    listPracticeClassScore.Add(data.LessonPodName + ": " + data.PrClassAvgMasteryIndex);
                }
                lessonFolder!.LessonPods = listLessonPod;
                listLessonFolder.Add(lessonFolder);
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
                responseData.LessonPodName = listLessonPodName;
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
                responseData.StudentCourseAverage = testAverage;
                responseData.PracticeStudentCourseAverage = practiceAverage;
                responseData.LessonFolders = listLessonFolder;
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
