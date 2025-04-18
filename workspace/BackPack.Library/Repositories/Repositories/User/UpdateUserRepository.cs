using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class UpdateUserRepository : GenericRepository, IUpdateUserRepository
    {
        #region UpdateUser       
        public async Task<BaseResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            BaseResponse response = new();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();                
                userParameters.Add("ip_login_name", request.LoginName, DbType.String);
                userParameters.Add("ip_fname", request.FName, DbType.String);
                userParameters.Add("ip_lname", request.LName, DbType.String);
                userParameters.Add("ip_email_id", request.EmailID, DbType.String);
                userParameters.Add("ip_gmail_id", request.GmailID, DbType.String);
                userParameters.Add("ip_phone_no", request.PhoneNo, DbType.String);
                userParameters.Add("ip_is_primary", (request.PrimaryFlag == 1), DbType.Boolean);
                userParameters.Add("ip_role", request.UserType, DbType.String);
                userParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.UpdateUser, DbType.String);
                userParameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.UpdateUser, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var userResult = userParameters.Get<int>("op_status_id"); 
                #endregion              

                #region Response
                if (userResult > 0)
                {
                    await sqlTransaction.CommitAsync();
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = UserMessage.UserUpdated;
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                }
                return response;
                #endregion
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
