using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Responses.Master.Subject;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using BackPack.Library.Responses.District;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Master
{
    public class SubjectRepository : GenericRepository, ISubjectRepository
    {
        #region GetSubjectAsync         
        public async Task<SubjectResponse> GetSubjectAsync(int SubjectID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetSubject, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetSubjectResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new SubjectResponseData
                {
                    GetSubjectResult = recordSetData.FirstOrDefault()
                };

                #region Response
                SubjectResponse response = new()
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
                return new SubjectResponse
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
                return new SubjectResponse
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

        #region GetAllSubjectAsync         
        public async Task<AllSubjectResponse> GetAllSubjectAsync(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetSubjects, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllSubjectResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllSubjectResponseData
                {
                    GetAllSubjectResult = recordSetData.ToList()
                };

                #region Response
                AllSubjectResponse response = new()
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
                return new AllSubjectResponse
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
                return new AllSubjectResponse
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

        #region AllSubjectsByTeacherAsync
        public async Task<AllSubjectsByTeacherResponse> AllSubjectsByTeacherAsync(int DomainID, int TeacherID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllSubjectsByTeacherResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_teacher_id", TeacherID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetAllSubjectsByTeacher, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllSubjectsByTeacherData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllSubjectsByTeacherDataResult
                {
                    GetAllSubjectsByTeacherResult = recordSetData.ToList()
                };
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

        #region GetAllSubjectsByDomainAsync        
        public async Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetAllSubjectsByDomain, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllSubjectsByDomainResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllSubjectsByDomainResponseData
                {
                    GetAllSubjectsByDomainResult = recordSetData.ToList()
                };

                #region Response
                AllSubjectsByDomainResponse response = new()
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
                return new AllSubjectsByDomainResponse
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
                return new AllSubjectsByDomainResponse
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
