using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Grade;
using BackPack.Library.Responses.Grade;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Grade
{
    public class GradeRepository : GenericRepository, IGradeRepository
    {
        #region GetGradeAsync         
        public async Task<GradeResponse> GetGradeAsync(int GradeID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_grade_id", GradeID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetGrade, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetGradeResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new GradeResponseData
                {
                    GetGradeResult = recordSetData.First()
                };

                #region Response
                GradeResponse response = new()
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
                return new GradeResponse
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
                return new GradeResponse
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

        #region GetAllGradeAsync         
        public async Task<AllGradeResponse> GetAllGradeAsync(int DomainID)
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

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetGrades, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllGradesResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllGradeResponseData
                {
                    GetAllGradesResult = recordSetData.ToList()
                };

                #region Response
                AllGradeResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = new List<AllGradeResponseData> { responseData }
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllGradeResponse
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
                return new AllGradeResponse
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
