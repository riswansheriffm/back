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
    public class CRSubjectSummaryTagReportRepository : GenericRepository, ICRSubjectSummaryTagReportRepository
    {
        #region CRSubjectSummaryTagReportAsync        
        public async Task<CRSubjectSummaryTagReportResponse> CRSubjectSummaryTagReportAsync(int SubjectID, int StudentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRSubjectSummaryTagReportResponse response = new();
            CRSubjectSummaryTagReportData responseData = new();
            List<CRSubjectSummaryTagReportFolderData> listFolderData = new();

            try
            {
                #region Set parameters            
                var LessonParameters = new DynamicParameters();
                LessonParameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                LessonParameters.Add("op_tag_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                LessonParameters.Add("op_lesson_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var dbLessonResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + "get_tags_by_subject", LessonParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                var outRecordSet = LessonParameters.Get<string>("op_tag_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRSubjectSummaryTagReportTagData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var outSummaryRecordSet = LessonParameters.Get<string>("op_lesson_record_set");
                string summaryRecordSetQuery = GlobalHelper.StringToString(outSummaryRecordSet);
                var summaryRecordSetData = await dbConnection.QueryAsync<CRSubjectSummaryTagReportLessonQueryData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listTagName = recordSetData.ToList();
                var listLesson = summaryRecordSetData.ToList();
                #endregion

                #region Set response data structure
                if (listLesson.Any())
                {
                    foreach (var lesson in listLesson)
                    {
                        #region Set parameters            
                        var parameters = new DynamicParameters();
                        parameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                        parameters.Add("ip_lesson_id", lesson.LessonID, DbType.Int32);
                        parameters.Add("ip_student_id", StudentID, DbType.Int32);
                        parameters.Add("op_lesson_details", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        parameters.Add("op_tag_summary", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        parameters.Add("op_lesson_tags", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        #endregion

                        var dbResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + "get_subject_summary_tag_report", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                        var outTagRecordSet = parameters.Get<string>("op_lesson_details");
                        string recordTagSetQuery = GlobalHelper.StringToString(outTagRecordSet);
                        var recordTagSetData = await dbConnection.QueryAsync<CRSubjectSummaryTagReportLessonQueryData>(recordTagSetQuery, sqlTransaction, commandType: CommandType.Text);

                        var outSummaryTagRecordSet = parameters.Get<string>("op_tag_summary");
                        string summaryRecordTagSetQuery = GlobalHelper.StringToString(outSummaryTagRecordSet);
                        var summaryRecordTagSetData = await dbConnection.QueryAsync<CRSubjectSummaryTagReportLessonQueryData>(summaryRecordTagSetQuery, sqlTransaction, commandType: CommandType.Text);

                        var outSummaryListTagRecordSet = parameters.Get<string>("op_lesson_tags");
                        string summaryRecordListTagSetQuery = GlobalHelper.StringToString(outSummaryListTagRecordSet);
                        var summaryRecordListTagSetData = await dbConnection.QueryAsync<CRSubjectSummaryTagReportLessonQueryData>(summaryRecordListTagSetQuery, sqlTransaction, commandType: CommandType.Text);


                        #region Read db response
                        var listLessonSummary = recordTagSetData.ToList();
                        var listTagSummary = summaryRecordTagSetData.ToList();
                        var listTag = summaryRecordListTagSetData.ToList();
                        #endregion

                        #region Set response
                        CRSubjectSummaryTagReportFolderData folderData = new();
                        List<string> listScore = new();
                        List<string> listFolderTag = new();
                        if (listLessonSummary.Any())
                        {
                            var lessonSummaryData = listLessonSummary.First();
                            folderData.FolderID = lessonSummaryData.LessonID;
                            folderData.FolderName = lessonSummaryData.LessonName;
                            folderData.AverageScore = lessonSummaryData.AverageScore + "%";
                        }
                        if (listTagSummary.Any())
                        {
                            foreach (var tagData in listTagSummary)
                            {
                                listScore.Add(tagData.Score + "%");
                            }
                            folderData.Score = listScore;
                        }
                        if (listTag.Any())
                        {
                            foreach (var tag in listTag)
                            {
                                listFolderTag.Add(tag.TagName);
                            }
                            folderData.Tags = listFolderTag;
                        }
                        listFolderData.Add(folderData);
                        #endregion
                    }
                }
                responseData.FolderData = listFolderData;
                responseData.Tags = listTagName;
                #endregion

                #region Response
                await sqlTransaction.CommitAsync();
                response.Data = new CRSubjectSummaryTagReport
                {
                    GetCRSubjectSummaryTagReportResult = responseData
                };
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
