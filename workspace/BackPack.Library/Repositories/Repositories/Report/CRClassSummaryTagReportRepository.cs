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
    public class CRClassSummaryTagReportRepository : GenericRepository, ICRClassSummaryTagReportRepository
    {
        #region CRClassSummaryTagReportAsync        
        public async Task<CRClassSummaryTagReportResponse> CRClassSummaryTagReportAsync(int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRClassSummaryTagReportResponse response = new();
            CRClassSummaryTagReportData responseData = new();

            try
            {
                #region Set parameters            
                var tagParameters = new DynamicParameters();
                tagParameters.Add("ip_course_id", CourseID, DbType.Int32);
                tagParameters.Add("op_tag_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                tagParameters.Add("op_student_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                
                var dbTagResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + "get_tags_by_class", tagParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                var outRecordSet = tagParameters.Get<string>("op_tag_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRClassSummaryTagReportTagData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var outSummaryRecordSet = tagParameters.Get<string>("op_student_record_set");
                string summaryRecordSetQuery = GlobalHelper.StringToString(outSummaryRecordSet);
                var summaryRecordSetData = await dbConnection.QueryAsync<CRClassSummaryTagReportTagQueryData>(summaryRecordSetQuery, sqlTransaction, commandType: CommandType.Text);


                #region Read db tag response
                var listTag = recordSetData.ToList();
                var listStudent = summaryRecordSetData.ToList();
                #endregion

                #region Set response data structure                
                List<CRClassSummaryTagReportStudentData> listData = new();
                if (listStudent.Any())
                {
                    foreach (var student in listStudent)
                    {
                        #region Set parameters            
                        var parameters = new DynamicParameters();
                        parameters.Add("ip_course_id", CourseID, DbType.Int32);
                        parameters.Add("ip_student_id", student.StudentID, DbType.Int32);
                        parameters.Add("op_student_details", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        parameters.Add("op_tag_summary", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        #endregion

                        var dbResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + "get_class_summary_tag_report", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                        var outTagRecordSet = parameters.Get<string>("op_student_details"); 
                        string recordTagSetQuery = GlobalHelper.StringToString(outTagRecordSet);
                        var recordTagSetData = await dbConnection.QueryAsync<CRClassSummaryTagReportStudentQueryData>(recordTagSetQuery, sqlTransaction, commandType: CommandType.Text);

                        var outSummaryTagRecordSet = parameters.Get<string>("op_tag_summary");
                        string summaryRecordTagSetQuery = GlobalHelper.StringToString(outSummaryTagRecordSet);
                        var summaryRecordTagSetData = await dbConnection.QueryAsync<CRClassSummaryTagReportStudentTagQueryData>(summaryRecordTagSetQuery, sqlTransaction, commandType: CommandType.Text);
                         
                         
                        var listStudentName = recordTagSetData.ToList();
                        var listTagData = summaryRecordTagSetData.ToList();

                        CRClassSummaryTagReportStudentData tagData = new();
                        List<string> listScore = new();
                        if (listTagData.Any())
                        {
                            foreach (var tag in listTagData)
                            {
                                listScore.Add(tag.Score + "%");
                            }
                            tagData.StudentScore = listScore;
                        }
                        if (listStudentName.Any())
                        {
                            var data = listStudentName.First();
                            tagData.StudentID = data.ID;
                            tagData.FullName = data.FullName;
                            tagData.LoginName = data.LoginName;
                            listData.Add(tagData);
                        }
                    }
                    responseData.StudentData = listData;
                }
                responseData.Tags = listTag;
                #endregion

                #region Response
                await sqlTransaction.CommitAsync();
                response.Data = new CRClassSummaryTagReport
                {
                    GetCRClassSummaryTagReportResult = responseData
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
