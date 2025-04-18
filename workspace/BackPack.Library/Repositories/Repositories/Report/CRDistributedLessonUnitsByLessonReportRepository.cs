using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class CRDistributedLessonUnitsByLessonReportRepository : GenericRepository, ICRDistributedLessonUnitsByLessonReportRepository
    {
        #region CRDistributedLessonUnitsByLessonReportAsync        
        public async Task<CRDistributedLessonUnitsByLessonReportResponse> CRDistributedLessonUnitsByLessonReportAsync(int AuthorID, int LessonID, int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRDistributedLessonUnitsByLessonReportResponse response = new();
            CRDistributedLessonUnitsByLessonReportData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", AuthorID, DbType.Int32);
                parameters.Add("ip_lesson_id", LessonID, DbType.Int32);
                parameters.Add("ip_course_id", CourseID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_past_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_summary_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetCrDistributedLessonUnitsByLessonReport, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRDistributedLessonUnitsByLessonReportLessonData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outPastRecordSet = parameters.Get<string>("op_past_record_set");
                string pastRecordSetQuery = GlobalHelper.StringToString(outPastRecordSet);
                var pastRecordSetData = await dbConnection.QueryAsync<CRDistributedLessonUnitsByLessonReportLessonData>(pastRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSummaryRecordSet = parameters.Get<string>("op_summary_record_set");
                string summaryRecordSetQuery = GlobalHelper.StringToString(outSummaryRecordSet);
                var summaryRecordSetData = await dbConnection.QueryAsync<CRDistributedLessonUnitsByLessonReportData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listUpcomingAssignment = recordSetData.ToList();
                var listPastAssignment = pastRecordSetData.ToList();
                var lessonSummary = summaryRecordSetData.ToList();
                #endregion

                #region Set response data structure
                if (lessonSummary.Count > 0)
                {
                    responseData = lessonSummary.First();
                    responseData.UpcomingAssignments = listUpcomingAssignment;
                    responseData.PastAssignments = listPastAssignment;
                }

                var responseResult = new CRDistributedLessonUnitsByLessonReportResult
                {
                    GetCRDistributedLessonUnitsByLessonReportResult = responseData
                };
                #endregion

                #region Response
                response.Data = responseResult;
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
