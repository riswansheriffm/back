using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Library.Responses.User;
using static BackPack.Library.Responses.LessonPod.LessonFoldersBySubjectResponseDataResult;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class LessonFoldersBySubjectRepository : GenericRepository, ILessonFoldersBySubjectRepository
    {
        #region GetLessonFoldersBySubjectAsync
        public async Task<LessonFoldersBySubjectResponse> GetLessonFoldersBySubjectAsync(int DomainID, int StudentID, int SubjectID, int ParentID, int ChapterID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();                
                parameters.Add("ip_student_id", StudentID, DbType.Int32);
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                parameters.Add("ip_parent_id", ParentID, DbType.Int32);
                parameters.Add("ip_chapter_id", ChapterID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetBpLessonFoldersBySubject, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<LessonFoldersBySubjectResponseData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new LessonFoldersBySubjectResponseDataResult
                {
                    GetBPLessonFoldersBySubjectResult = recordSetData.ToList()
                };

                #region Response                
                LessonFoldersBySubjectResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new LessonFoldersBySubjectResponse
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = CommonMessage.ExceptionMessage,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.Message + " : " + ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
                return new LessonFoldersBySubjectResponse
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = CommonMessage.ExceptionMessage,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.Message + " : " + ex.StackTrace
                };
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
