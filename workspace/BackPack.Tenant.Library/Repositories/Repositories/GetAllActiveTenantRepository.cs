using BackPack.Dependency.Library.Messages;
using BackPack.Tenant.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using BackPack.Tenant.Library.Repositories.Interfaces;
using Npgsql;
using NpgsqlTypes;
using BackPack.Tenant.Library.Constants;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class GetAllActiveTenantRepository(IConfiguration configuration) : GenericRepository(configuration), IGetAllActiveTenantRepository
    {
        public async Task<GetAllActiveDomainsResponse> GetAllActiveTenantsAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetAllActiveTenants, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllPublicActiveDomainsResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new GetAllActiveDomainsResponseData
                {
                    GetAllActiveDomainsResult = recordSetData.ToList()
                };

                #region Response
                GetAllActiveDomainsResponse response = new()
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
                return new GetAllActiveDomainsResponse
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
                return new GetAllActiveDomainsResponse
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
