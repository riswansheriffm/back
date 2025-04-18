using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Responses.School;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.School
{
    public class SchoolRepository : GenericRepository, ISchoolRepository
    {
        #region GetSchoolAsync       
        public async Task<SchoolResponse> GetSchoolAsync(int SchoolID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_user_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetSchool, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetSchoolResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outUserRecordSet = parameters.Get<string>("op_user_record_set");
                string userRecordSetQuery = GlobalHelper.StringToString(outUserRecordSet);
                var userRecordSetData = await dbConnection.QueryAsync<SchoolsList>(userRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new SchoolResponseData
                {
                    GetSchoolResult = new GetSchoolResult
                    {
                        SchoolDesc = recordSetData?.FirstOrDefault()?.SchoolDesc,
                        SchoolName = recordSetData?.FirstOrDefault()?.SchoolName,
                        SchoolID = recordSetData?.FirstOrDefault()?.SchoolID ?? 0,
                        UO = userRecordSetData.ToList()
                    }
                };

                #region Response
                SchoolResponse response = new()
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
                return new SchoolResponse
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
                return new SchoolResponse
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

        #region GetAllSchoolAsync         
        public async Task<AllSchoolResponse> GetAllSchoolAsync(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetSchools, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllSchoolsResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllResponseData
                {
                    GetAllSchoolsResult = recordSetData.ToList()
                };

                #region Response
                AllSchoolResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = new List<AllResponseData> { responseData }
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllSchoolResponse
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
                return new AllSchoolResponse
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
