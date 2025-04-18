using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.Token;
using Dapper;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class TokenRepository : GenericRepository, ITokenRepository
    {
        #region CreateRefreshToken
        public async Task<int> CreateRefreshToken(TokenRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_user_id", request.UserID, DbType.Int32);
                parameters.Add("ip_user_type", request.UserType, DbType.String);
                parameters.Add("ip_token_hash", request.TokenHash, DbType.String);
                parameters.Add("ip_token_salt", request.TokenSalt, DbType.String);
                parameters.Add("ip_expiry_days", request.RefreshTokenExpirationInDays, DbType.Int32);
                parameters.Add("op_user_count", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateRefreshToken, parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("op_user_count");
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
    }
}
