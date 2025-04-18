
using BackPack.Dependency.Library.Helpers;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using Dapper;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class LoginRepository : GenericRepository, ILoginRepository
    {
        #region UserForLogin
        public async Task<Tuple<IEnumerable<dynamic>, int>> UserForLogin(LoginRequest request, string UserType)
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
                parameters.Add("ip_user_type", UserType, DbType.String);
                parameters.Add("op_password_count", NpgsqlDbType.Integer, direction: ParameterDirection.Output);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var user = await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetUserForLogin, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int PasswordCount = parameters.Get<int>("op_password_count");

                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
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
        public async Task<int> UpdateWrongPassword(LoginRequest request, string UserType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_district_name", request.DistrictName, DbType.String);
                parameters.Add("ip_user_type", UserType, DbType.String);
                parameters.Add("ip_user_type_id", (UserType.Equals("teacher", StringComparison.CurrentCultureIgnoreCase)) ? 1 : 2, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.UpdateWrongPassword, DbType.String);
                parameters.Add("op_wrong_password_count", NpgsqlDbType.Integer, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.UpdateWrongPassword, parameters, commandType: CommandType.StoredProcedure);
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
        public async Task ResetWrongPassword(LoginRequest request, string UserType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_district_name", request.DistrictName, DbType.String);
                parameters.Add("ip_user_type", UserType, DbType.String);
                parameters.Add("ip_user_type_id", (UserType.Equals("teacher", StringComparison.CurrentCultureIgnoreCase)) ? 1 : 2, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.ResetWrongPassword, DbType.String);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.ResetWrongPassword, parameters, commandType: CommandType.StoredProcedure);

            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
