using BackPack.Dependency.Library.Messages;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using BackPack.Tenant.Library.Constants;
using NpgsqlTypes;
using Npgsql;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class GetAllTenantRepository(IConfiguration configuration) : GenericRepository(configuration), IGetAllTenantRepository
    {
        public async Task<GetAllDistrictResponse> GetAllTenantsAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetTenants, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllDistrictsResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllDistrictsResponseData
                {
                    GetAllDistrictsResults = recordSetData.ToList()
                };

                #region Response                
                GetAllDistrictResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };

                await sqlTransaction.CommitAsync();
                return response;
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new GetAllDistrictResponse
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
                return new GetAllDistrictResponse
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
    }
}
