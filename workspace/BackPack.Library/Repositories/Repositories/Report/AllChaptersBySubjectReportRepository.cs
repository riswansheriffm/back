using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class AllChaptersBySubjectReportRepository : GenericRepository, IAllChaptersBySubjectReportRepository
    {
        #region AllChaptersBySubjectReportAsync
        public async Task<AllChaptersBySubjectReportResponse> AllChaptersBySubjectReportAsync(int SubjectID, int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllChaptersBySubjectReportResponse response = new();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                parameters.Add("ip_course_id", CourseID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_summary_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetAllChaptersBySubjectReport, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllChaptersBySubjectReportChapterData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSummaryRecordSet = parameters.Get<string>("op_summary_record_set");
                string summaryRecordSetQuery = GlobalHelper.StringToString(outSummaryRecordSet);
                var summaryRecordSetData = await dbConnection.QueryAsync<AllChaptersBySubjectReportData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listChapter = recordSetData.ToList();
                var subjectSummary = summaryRecordSetData.ToList();
                #endregion

                #region Declaration
                AllChaptersBySubjectReportData responseData = new();
                #endregion

                #region Set response data structure
                if (subjectSummary.Count > 0)
                {
                    responseData = subjectSummary.First();
                    responseData.NoOfChapters = listChapter.Count;
                    responseData.Chapters = listChapter;
                }

                var responseResult = new AllChaptersBySubjectReportResult
                {
                    GetAllChaptersBySubjectReportResult = responseData
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
