using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class AllDistributedLessonpodsByTeacherRepository : GenericRepository, IAllDistributedLessonpodsByTeacherRepository
    {
        #region AllDistributedLessonpodsByTeacherAsync
        public async Task<AllDistributedLessonpodsByTeacherResponse> AllDistributedLessonpodsByTeacherAsync(int LoginID, int LessonUnitID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllDistributedLessonpodsByTeacherResponse response = new();
            List<AllDistributedLessonpodsByTeacherData> listResponseData = [];

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", LoginID, DbType.Int32);
                parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllDistributedLessonpodsByTeacherByLessonpod, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllDistributedLessonpodsByTeacherQueryResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response data
                if (recordSetData.Any())
                {
                    foreach (var data in recordSetData)
                    {
                        AllDistributedLessonpodsByTeacherData responseData = new();

                        responseData.LessonUnitDistID = data.LessonUnitDistID;
                        responseData.LessonUnitName = data.LessonUnitName;
                        responseData.Description = data.Description;
                        responseData.LessonfolderName = data.LessonfolderName;
                        responseData.ClassName = data.ClassName;
                        responseData.SubjectName = data.SubjectName;
                        responseData.Targetdate = data.Targetdate;
                        responseData.TargetdatePassed = data.TargetdatePassed;
                        responseData.StudentNameList = data.Students.Split(',').ToList();

                        listResponseData.Add(responseData);
                    }
                }
                #endregion

                AllDistributedLessonpodsByTeacherResult returnData = new()
                {
                    GetAllDistributedLessonpodsByTeacherResult = listResponseData
                };

                #region Response
                response.Data = returnData;
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
