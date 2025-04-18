using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Dashboard;
using BackPack.Library.Responses.Dashboard;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;

namespace BackPack.Library.Repositories.Repositories.Dashboard
{
    public class ClassRoomDashboardRepository : GenericRepository, IClassRoomDashboardRepository
    {
        #region ClassRoomDashboardResponseAsync        
        public async Task<ClassRoomDashboardResponse> ClassRoomDashboardResponseAsync(int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            ClassRoomDashboardResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", LoginID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_summary_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_teacher_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + "get_cr_teacher_dashboard", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = "";
                if (outRecordSet != "")
                {
                    recordSetQuery = $"FETCH ALL IN \"{outRecordSet}\"";
                }
                var recordSetData = await dbConnection.QueryAsync<ClassRoomDashboardCourseData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSummaryRecordSet = parameters.Get<string>("op_summary_record_set");
                string summaryRecordSetQuery = "";
                if (outSummaryRecordSet != "")
                {
                    summaryRecordSetQuery = $"FETCH ALL IN \"{outSummaryRecordSet}\"";
                }
                var summaryRecordSetData = await dbConnection.QueryAsync<ClassRoomDashboardData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outTeacherRecordSet = parameters.Get<string>("op_teacher_record_set");
                string teacherRecordSetQuery = "";
                if (outTeacherRecordSet != "")
                {
                    teacherRecordSetQuery = $"FETCH ALL IN \"{outTeacherRecordSet}\"";
                }
                var teacherRecordSetData = await dbConnection.QueryAsync<ClassRoomDashboardTeacherData>(teacherRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listCourse = recordSetData.ToList();
                var classSummary = summaryRecordSetData.ToList();
                var teacher = teacherRecordSetData.ToList();
                #endregion

                #region Declaration
                ClassRoomDashboardData responseResult = new();
                #endregion

                #region Set response data structure
                if (classSummary.Any())
                {
                    responseResult = classSummary.First();
                    responseResult.Course = listCourse;
                }

                if (teacher.Any())
                {
                    var teacherData = teacher.First();
                    responseResult.FullName = teacherData.FullName;
                }
                var responseData = new ClassRoomDashboardResult
                {
                    GetTeacherDashboardResult = responseResult
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
