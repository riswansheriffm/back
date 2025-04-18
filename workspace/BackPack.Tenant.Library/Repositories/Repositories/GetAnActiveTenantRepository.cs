
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class GetAnActiveTenantRepository(IConfiguration configuration) : GenericRepository(configuration), IGetAnActiveTenantRepository
    {
        public async Task<Guid> GetAnActiveTenantAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("op_tenant_id", NpgsqlDbType.Uuid, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetActiveTenant, parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<Guid>("op_tenant_id");
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
    }
}
