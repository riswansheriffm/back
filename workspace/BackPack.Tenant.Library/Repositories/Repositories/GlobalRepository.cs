
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class GlobalRepository(IConfiguration configuration) : GenericRepository(configuration), IGlobalRepository
    {
        #region GetLoginNameByID
        public async Task<string> GetLoginNameByID(int userID, string userType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_user_id", userID, DbType.Int32);
                parameters.Add("ip_user_type", userType, DbType.String);
                parameters.Add("op_return_login_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "get_login_name_by_id", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("op_return_login_name");
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
