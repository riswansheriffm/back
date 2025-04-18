using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class LoginRepository(IConfiguration configuration) : GenericRepository(configuration), ILoginRepository
    {
        #region UserForLogin
        public async Task<Tuple<IEnumerable<dynamic>, int>> UserForLogin(LoginRequest request, string userType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_district_name", request.DistrictName, DbType.String);
                parameters.Add("ip_user_type", userType, DbType.String);
                parameters.Add("op_password_count", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetUserForLogin, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int PasswordCount = parameters.Get<int>("op_password_count");

                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = "";
                if (outRecordSet != "")
                {
                    recordSetQuery = $"FETCH ALL IN \"{outRecordSet}\"";
                }
                var recordSetData = await dbConnection.QueryAsync(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var response = new Tuple<IEnumerable<dynamic>, int>(recordSetData, PasswordCount);

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (Exception)
            {
                await sqlTransaction.RollbackAsync();
                return new Tuple<IEnumerable<dynamic>, int>(null!, 0);
            }
            finally
            {
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region UpdateWrongPassword
        public async Task<int> UpdateWrongPassword(LoginRequest request, string userType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters 
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_district_name", request.DistrictName, DbType.String);
                parameters.Add("ip_user_type", userType, DbType.String);
                parameters.Add("ip_activity_desc", LogMessage.UpdateWrongPassword, DbType.String);
                parameters.Add("op_wrong_password_count", NpgsqlDbType.Integer, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateWrongPassword, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("op_wrong_password_count");
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region ResetWrongPassword
        public async Task ResetWrongPassword(LoginRequest request, string userType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_district_name", request.DistrictName, DbType.String);
                parameters.Add("ip_user_type", userType, DbType.String);
                parameters.Add("ip_activity_desc", LogMessage.ResetWrongPassword, DbType.String);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.ResetWrongPassword, parameters, commandType: CommandType.StoredProcedure);
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
