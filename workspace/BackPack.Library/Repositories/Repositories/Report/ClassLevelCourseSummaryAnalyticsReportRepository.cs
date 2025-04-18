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
    public class ClassLevelCourseSummaryAnalyticsReportRepository : GenericRepository, IClassLevelCourseSummaryAnalyticsReportRepository
    {
        #region ClassLevelCourseSummaryAnalyticsReportAsync
        public async Task<ClassLevelCourseSummaryAnalyticsReportResponse> ClassLevelCourseSummaryAnalyticsReportAsync(int DomainID, int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            ClassLevelCourseSummaryAnalyticsReportResponse response = new();
            ClassLevelCourseSummaryAnalyticsReportData responseData = new();
            List<ClassLevelCourseSummaryAnalyticsReportStudent> listStudentActivity = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("DomainID", DomainID, DbType.Int32);
                parameters.Add("CourseID", CourseID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaStudents + "GetClassLevelCourseSummaryAnalyticsReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response
                var testSummary = dbResponse!.Read<AnalyticsReportSummaryAverageData>().ToList();
                var practiceSummary = dbResponse!.Read<AnalyticsReportSummaryAverageData>().ToList();
                var studentSummary = dbResponse!.Read<ClassLevelCourseSummaryAnalyticsReportQueryData>().ToList();
                #endregion

                #region Declaration   
                List<int> listStudentAverageTimeTaken = new();
                List<int> listPracticeStudentAverageTimeTaken = new();
                List<string> listStudentsLoginName = new();
                List<string> listStudentsFullName = new();
                List<string> listStudentAverageSpentTime = new();
                List<string> listStudentAverageScore = new();
                List<string> listStudentAverageTimelyCompletion = new();
                List<string> listStudentSpentTime = new();
                List<string> listStudentTimeTaken = new();
                List<string> listStudentScore = new();
                List<string> listStudentTimelyCompletion = new();
                List<string> listPracticeStudentAverageSpentTime = new();
                List<string> listPracticeStudentAverageScore = new();
                List<string> listPracticeStudentAverageTimelyCompletion = new();
                List<string> listPracticeStudentSpentTime = new();
                List<string> listPracticeStudentTimeTaken = new();
                List<string> listPracticeStudentScore = new();
                List<string> listPracticeStudentTimelyCompletion = new();
                #endregion

                #region Set response data structure
                if (testSummary.Any())
                {
                    var testData = testSummary.First();

                    responseData.AverageTimeSpent = testData.AverageTimeSpent;
                    responseData.AverageTimeTaken = testData.AverageTimeTaken;
                    responseData.AverageScore = testData.AverageScore;
                    responseData.AverageTimelyCompletion = testData.AverageTimelyCompletion;
                }
                if (practiceSummary.Any())
                {
                    var practiceData = practiceSummary.First();

                    responseData.PracticeAverageTimeSpent = practiceData.AverageTimeSpent;
                    responseData.PracticeAverageTimeTaken = practiceData.AverageTimeTaken;
                    responseData.PracticeAverageScore = practiceData.AverageScore;
                    responseData.PracticeAverageTimelyCompletion = practiceData.AverageTimelyCompletion;
                }
                if (studentSummary.Any())
                {
                    foreach (var studentData in studentSummary)
                    {
                        ClassLevelCourseSummaryAnalyticsReportStudent studentActivity = new();
                        studentActivity.StudentID = studentData.StudentID;
                        studentActivity.LoginName = studentData.LoginName;
                        studentActivity.FullName = studentData.FullName;
                        studentActivity.StudentScore = studentData.TestStudentAverageScore;
                        studentActivity.StudentTimeSpent = studentData.TestStudentAverageTimeSpent;
                        studentActivity.StudentTimeTaken = studentData.TestStudentTimeTaken;
                        studentActivity.StudentTimelyCompletion = studentData.TestStudentAverageTimelyCompletion;
                        studentActivity.PracticeStudentScore = studentData.PracticeStudentAverageScore;
                        studentActivity.PracticeStudentTimeSpent = studentData.PracticeStudentAverageTimeSpent;
                        studentActivity.PracticeStudentTimeTaken = studentData.PracticeStudentTimeTaken;
                        studentActivity.PracticeStudentTimelyCompletion = studentData.PracticeStudentAverageTimelyCompletion;
                        listStudentActivity.Add(studentActivity);
                        listStudentAverageTimeTaken.Add(int.Parse(studentData.TestStudentTimeTaken));
                        listPracticeStudentAverageTimeTaken.Add(int.Parse(studentData.PracticeStudentTimeTaken));
                        listStudentsLoginName.Add(studentData.LoginName);
                        listStudentsFullName.Add(studentData.FullName);
                        listStudentAverageSpentTime.Add(studentData.TestStudentAverageTimeSpent);
                        listStudentAverageScore.Add(studentData.TestStudentAverageScore);
                        listStudentAverageTimelyCompletion.Add(studentData.TestStudentAverageTimelyCompletion);
                        listStudentSpentTime.Add(studentData.FullName + ": " + studentData.TestStudentAverageTimeSpent);
                        listStudentTimeTaken.Add(studentData.FullName + ": " + studentData.TestStudentTimeTaken);
                        listStudentScore.Add(studentData.FullName + ": " + studentData.TestStudentAverageScore);
                        listStudentTimelyCompletion.Add(studentData.FullName + ": " + studentData.TestStudentAverageTimelyCompletion);
                        listPracticeStudentAverageSpentTime.Add(studentData.PracticeStudentAverageTimeSpent);
                        listPracticeStudentAverageScore.Add(studentData.PracticeStudentAverageScore);
                        listPracticeStudentAverageTimelyCompletion.Add(studentData.PracticeStudentAverageTimelyCompletion);
                        listPracticeStudentSpentTime.Add(studentData.FullName + ": " + studentData.PracticeStudentAverageTimeSpent);
                        listPracticeStudentTimeTaken.Add(studentData.FullName + ": " + studentData.PracticeStudentTimeTaken);
                        listPracticeStudentScore.Add(studentData.FullName + ": " + studentData.PracticeStudentAverageScore);
                        listPracticeStudentTimelyCompletion.Add(studentData.FullName + ": " + studentData.PracticeStudentAverageTimelyCompletion);
                    }
                    responseData.StudentSummary = listStudentActivity;
                    responseData.StudentsLoginName = listStudentsLoginName;
                    responseData.StudentsFullName = listStudentsFullName;
                    responseData.StudentAverageSpentTime = listStudentAverageSpentTime;
                    responseData.StudentAverageTimeTaken = listStudentAverageTimeTaken;
                    responseData.StudentAverageScore = listStudentAverageScore;
                    responseData.StudentAverageTimelyCompletion = listStudentAverageTimelyCompletion;
                    responseData.StudentSpentTime = listStudentSpentTime;
                    responseData.StudentTimeTaken = listStudentTimeTaken;
                    responseData.StudentScore = listStudentScore;
                    responseData.StudentTimelyCompletion = listStudentTimelyCompletion;
                    responseData.PracticeStudentAverageSpentTime = listPracticeStudentAverageSpentTime;
                    responseData.PracticeStudentAverageTimeTaken = listPracticeStudentAverageTimeTaken;
                    responseData.PracticeStudentAverageScore = listPracticeStudentAverageScore;
                    responseData.PracticeStudentAverageTimelyCompletion = listPracticeStudentAverageTimelyCompletion;
                    responseData.PracticeStudentSpentTime = listPracticeStudentSpentTime;
                    responseData.PracticeStudentTimeTaken = listPracticeStudentTimeTaken;
                    responseData.PracticeStudentScore = listPracticeStudentScore;
                    responseData.PracticeStudentTimelyCompletion = listPracticeStudentTimelyCompletion;
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
