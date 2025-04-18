using BackPack.Library.Helpers.LessonPod;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Responses.Master.Group;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class DistributedLessonPodRepository : GenericRepository, IDistributedLessonPodRepository
    {
        #region DistributedLessonPodAsync
        public async Task<DistributedLessonPodResponse> DistributedLessonPodAsync(int LessonUnitDistID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            DistributedLessonPodResponse response = new();
            DistributedLessonPodData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                parameters.Add("op_lessonpod_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_target_date_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_student_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetDistributedLessonUnit, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outLessonpodRecordSet = parameters.Get<string>("op_lessonpod_record_set");
                string lessonpodRecordSetQuery = GlobalHelper.StringToString(outLessonpodRecordSet);
                var lessonpodRecordSetData = await dbConnection.QueryAsync<DistributedLessonPodQueryResponse>(lessonpodRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var outTargetDateRecordSet = parameters.Get<string>("op_target_date_record_set");
                string targetDateRecordSetQuery = GlobalHelper.StringToString(outTargetDateRecordSet);
                var targetDateRecordSetData = await dbConnection.QueryAsync<DistributedLessonPodQueryResponse>(targetDateRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var outStudentRecordSet = parameters.Get<string>("op_student_record_set");
                string studentRecordSetQuery = GlobalHelper.StringToString(outStudentRecordSet);
                var studentRecordSetData = await dbConnection.QueryAsync<StudentList>(studentRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response                
                var lessonPod = lessonpodRecordSetData.ToList();
                var activity = targetDateRecordSetData.ToList();
                var listStudent = studentRecordSetData.ToList();
                #endregion

                #region Response data
                if (lessonPod.Any())
                {
                    var lessonData = lessonPod.First();

                    responseData.LessonUnitDistID = lessonData.LessonUnitDistID;
                    responseData.CourseID = lessonData.CourseID;
                    responseData.LessonName = lessonData.LessonName;
                    responseData.LessonDesc = lessonData.LessonDesc;
                    responseData.WhomToDistribute = lessonData.WhomToDistribute;
                    responseData.GroupIDs = lessonData.GroupIDs.Split(',').ToList();
                    responseData.Slides = LessonPodHelper.LessonPodSlideList(lessonData.LessonJson!);
                }

                if (activity.Any())
                {
                    var activityData = activity.First();

                    responseData.TargetDateOfCompletion = activityData.TargetDateOfCompletion;
                    responseData.TargetTimeOfCompletion = activityData.TargetTimeOfCompletion;
                }

                if (listStudent.Any())
                {
                    List<int> studentList = new();
                    foreach (var student in listStudent)
                    {
                        studentList.Add(student.StudentID);
                    }
                    responseData.StudentIDs = studentList;
                }
                #endregion

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
