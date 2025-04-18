
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using Dapper;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class PasswordRepository : GenericRepository, IPasswordRepository
    {
        #region CreatePasswordAsync
        public async Task<BaseResponse> CreatePasswordAsync(PasswordRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_from_user_id", request.FromUserID, DbType.Int32);
                parameters.Add("ip_to_user_id", request.ToUserID, DbType.Int32);
                parameters.Add("ip_user_type", request.UserType, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetMigratedUsers, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<MigratedUserResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                foreach (var user in recordSetData)
                {
                    var userPasswordSalt = Hash.GetSecureSalt();
                    var userPasswordHash = Hash.HashUsingPbkdf2(user.Password, userPasswordSalt);

                    #region Set Parameters
                    var passwordParameters = new DynamicParameters();
                    passwordParameters.Add("ip_user_id", user.UserID, DbType.Int32);
                    passwordParameters.Add("ip_password_hash", userPasswordHash, DbType.String);
                    passwordParameters.Add("ip_password_salt", Convert.ToBase64String(userPasswordSalt).ToString(), DbType.String);
                    passwordParameters.Add("ip_user_type", "Teacher", DbType.String);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateMigratedUserPassword, passwordParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                }

                #region Response
                await sqlTransaction.CommitAsync();

                return new BaseResponse()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = UserMessage.MigratedUserPasswordCreated
                };
                #endregion

            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    MessageID = CommonMessage.ExceptionID,
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

                return new BaseResponse()
                {
                    MessageID = CommonMessage.ExceptionID,
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
