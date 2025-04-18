using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.Token;
using BackPack.Dependency.Library.Responses;
using Dapper;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class ValidateRefreshTokenRepository : GenericRepository, IValidateRefreshTokenRepository
    {
        #region ValidateRefreshTokenAsync
        public async Task<BaseResponse> ValidateRefreshTokenAsync(RefreshTokenRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            var response = new BaseResponse();

            try
            {
                #region Set parameters                
                var refreshTokenParameters = new DynamicParameters();
                refreshTokenParameters.Add("ip_user_id", request.UserID, DbType.Int32);
                refreshTokenParameters.Add("ip_user_type", request.UserType, DbType.String);
                refreshTokenParameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetUserRefreshToken, refreshTokenParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = refreshTokenParameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var refreshToken = await dbConnection.QueryAsync(recordSetQuery, sqlTransaction, commandType: CommandType.Text);


                if (!refreshToken.Any())
                {
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = UserMessage.RefreshTokenFail;
                    await sqlTransaction.RollbackAsync();
                    return response;
                }
                var refreshTokenToValidateHash = Hash.HashUsingPbkdf2(request.RefreshToken ?? "", Convert.FromBase64String(refreshToken.ToList()[0].TokenSalt!.ToString() ?? ""));
                if (refreshToken.ToList()[0].TokenHash!.ToString() != refreshTokenToValidateHash)
                {
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = UserMessage.RefreshTokenFail;
                    await sqlTransaction.RollbackAsync();
                    return response;
                }

                response.MessageID = CommonMessage.SuccessID;
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.DomainName = refreshToken.ToList()[0].Role;
                response.ResultCount = refreshToken.ToList()[0].DomainID;

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                response.MessageID = CommonMessage.ExceptionID;
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
                response.MessageID = CommonMessage.ExceptionID;
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
