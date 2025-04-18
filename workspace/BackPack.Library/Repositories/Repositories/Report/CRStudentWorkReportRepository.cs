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
    public class CRStudentWorkReportRepository : GenericRepository, ICRStudentWorkReportRepository
    {
        #region CRStudentWorkReportAsync        
        public async Task<CRStudentWorkReportResponse> CRStudentWorkReportAsync(int LessonUnitDistID, int StudentID, int AuthorID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRStudentWorkReportResponse response = new();
            CRStudentWorkReportData responseResult = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_author_id", AuthorID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_lesson_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetCrStudentWork, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRStudentWorkReportWorkData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outLessonRecordSet = parameters.Get<string>("op_lesson_record_set");
                string lessonRecordSetQuery = GlobalHelper.StringToString(outLessonRecordSet);
                var lessonRecordSetData = await dbConnection.QueryAsync<CRStudentWorkReportLessonData>(lessonRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listStudentActivity = recordSetData.ToList();
                var lessonData = lessonRecordSetData.ToList();
                #endregion

                #region Set response data structure
                if (listStudentActivity.Any())
                {
                    if (lessonData.Any())
                    {
                        responseResult.LessonUnit = lessonData.First();
                    }
                    responseResult.LessonActivity = listStudentActivity;
                }

                var responseData = new CRStudentWorkReportResult
                {
                    GetCRStudentWorkResult = responseResult,
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
