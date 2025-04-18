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
    public class CRLessonFoldersBySubjectReportRepository : GenericRepository, ICRLessonFoldersBySubjectReportRepository
    {
        #region CRLessonFoldersBySubjectReportAsync        
        public async Task<CRLessonFoldersBySubjectReportResponse> CRLessonFoldersBySubjectReportAsync(int DomainID, int AuthorID, int SubjectID, int CourseID, int ChapterID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRLessonFoldersBySubjectReportResponse response = new();
            CRLessonFoldersBySubjectReportData responseResult = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", AuthorID, DbType.Int32);
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                parameters.Add("ip_course_id", CourseID, DbType.Int32);
                parameters.Add("ip_chapter_id", ChapterID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_summary_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetCrLessonFoldersBySubjectReport, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRLessonFoldersBySubjectReportLessonSummaryData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSummaryRecordSet = parameters.Get<string>("op_summary_record_set");
                string summaryRecordSetQuery = GlobalHelper.StringToString(outSummaryRecordSet);
                var summaryRecordSetData = await dbConnection.QueryAsync<CRLessonFoldersBySubjectReportData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listLessonFolder = recordSetData.ToList();
                var lessonSummary = summaryRecordSetData.ToList();
                #endregion

                #region Set response data structure
                if (listLessonFolder.Any())
                {
                    if (lessonSummary.Any())
                    {
                        responseResult = lessonSummary.First();
                    }
                    responseResult.LessonFolders = listLessonFolder;
                    responseResult.NoOfLessonFolders = listLessonFolder.Count();
                }

                var responseData = new CRLessonFoldersBySubjectReportResult
                {
                    GetCRLessonFoldersBySubjectReportResult = responseResult,
                };
                #endregion

                #region Response
                await sqlTransaction.CommitAsync();
                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

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
