using BackPack.Dependency.Library.Messages;
using BackPack.Tenant.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.Extensions.Configuration;
using BackPack.Tenant.Library.Repositories.Interfaces;
using Npgsql;
using NpgsqlTypes;
using BackPack.Tenant.Library.Constants;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class GetTenantByTenantIDRepository(IConfiguration configuration) : GenericRepository(configuration), IGetTenantByTenantIDRepository
    {
        #region GetTenantDBConnection 
        public async Task<GetTenantByTenantNameResponse> GetTenantDBConnection(Guid tenantID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            GetTenantByTenantNameResponse response = new();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_tenant_id", tenantID, DbType.Guid);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetTenantByTenantId, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetTenantByTenantNameData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Response
                if (recordSetData.Any())
                {
                    response.Data = recordSetData.First();
                }
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
