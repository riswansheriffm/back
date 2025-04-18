using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Dashboard;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Responses.Dashboard;
using BackPack.Library.Constants;
using NpgsqlTypes;
using Npgsql;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Dashboard
{
    public class StudentDashboardRepository : GenericRepository, IStudentDashboardRepository
    {
        #region StudentDashboardResponseAsync
        public async Task<StudentDashboardResponse> StudentDashboardResponseAsync(int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            StudentDashboardResponse response = new();
            StudentDashboardDataResponse result = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_student_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_status_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + "get_bp_student_dashboard", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<StudentDashboardCourseResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outStudentRecordSet = parameters.Get<string>("op_student_record_set");
                string studentRecordSetQuery = GlobalHelper.StringToString(outStudentRecordSet);
                var studentRecordSetData = await dbConnection.QueryAsync<StudentDashboardDataReadResponse>(studentRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outStatusRecordSet = parameters.Get<string>("op_status_record_set");
                string statusRecordSetQuery = GlobalHelper.StringToString(outStatusRecordSet);
                var statusRecordSetData = await dbConnection.QueryAsync<StudentDashboardDataReadResponse>(statusRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response                
                var listCourse = recordSetData.ToList();
                var student = studentRecordSetData.ToList();
                var completion = statusRecordSetData.ToList();

                if (completion.Count > 0) result.CompletionPercentage = completion[0].CompletionPercentage;
                if (student.Count > 0)
                {
                    result.StudentID = student[0].ID;
                    result.UserName = student[0].FullName;
                }
                result.Course = listCourse;

                var responseData = new StudentDashboardDataResult
                {
                    GetBPStudentDashboardResult = result
                };

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
