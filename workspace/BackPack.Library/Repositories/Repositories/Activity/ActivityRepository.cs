using BackPack.Library.Repositories.Interfaces.Activity;
using BackPack.Library.Responses.Activity;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using NpgsqlTypes;
using Npgsql;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Activity
{
    public class ActivityRepository : GenericRepository, IActivityRepository
    {
        #region BackpackActivityForStudentAsync
        public async Task<BackpackActivityForStudentResponse> BackpackActivityForStudentAsync(int StudentID, int ContentID, int ParentID)
        {
            BackpackActivityForStudentResponse response = new();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_content_id", ContentID, DbType.Int32);
                parameters.Add("ip_parent_id", ParentID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_feedback_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_status_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_other_activity_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetBackpackActivityForStudent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<BackpackActivityForStudentData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outFeedbackRecordSet = parameters.Get<string>("op_feedback_record_set");
                string feedbackRecordSetQuery = GlobalHelper.StringToString(outFeedbackRecordSet);
                var feedbackRecordSetData = await dbConnection.QueryAsync<BackpackActivityForStudentFeedback>(feedbackRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outStatusRecordSet = parameters.Get<string>("op_status_record_set");
                string statusRecordSetQuery = GlobalHelper.StringToString(outStatusRecordSet);
                var statusRecordSetData = await dbConnection.QueryAsync<BackpackActivityForStudentParentContent>(statusRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outOtherActivityRecordSet = parameters.Get<string>("op_other_activity_record_set");
                string otherActivityRecordSetQuery = GlobalHelper.StringToString(outOtherActivityRecordSet);
                var otherActivityRecordSetData = await dbConnection.QueryAsync<BackpackActivityStudentData>(otherActivityRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response                
                var listActivity = recordSetData.ToList();
                var listFeedback = feedbackRecordSetData.ToList();
                var listParentContent = statusRecordSetData.ToList();
                #endregion

                #region Response data                
                BackpackActivityForStudentData activityData = new();
                if (listActivity.Count > 0)
                {
                    activityData = listActivity[0];
                    activityData.Feedback = listFeedback;
                    activityData.ContainedView = listParentContent;
                    activityData.OtherActivities = otherActivityRecordSetData.ToList();
                    activityData.OtherActivities.Add(listActivity[0]);
                    activityData.OtherActivities.Reverse();
                }

                var responseData = new BackpackActivityForStudentDataResult
                {
                    GetBackpackActivityForStudentResult = activityData
                };
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
