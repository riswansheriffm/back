
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class CreateSuperUserRepository : GenericRepository, ICreateSuperUserRepository
    {
        #region CreateSuperUserAsync        
        public async Task<BaseResponse> CreateSuperUserAsync(SuperUserRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();
            
            try
            {
                #region Set user parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_first_name", request.FName, DbType.String);
                parameters.Add("ip_last_name", request.LName, DbType.String);
                parameters.Add("ip_email_id", request.EmailID, DbType.String);
                parameters.Add("ip_phone_number", request.PhoneNo, DbType.String);
                parameters.Add("ip_role", request.UserType, DbType.String);
                parameters.Add("ip_is_active", (request.ActiveFlag == 1), DbType.Boolean);
                parameters.Add("ip_is_primary", (request.PrimaryFlag == 1), DbType.Boolean);
                parameters.Add("ip_school_id", request.SchoolID, DbType.Int32);
                parameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                parameters.Add("op_user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_domain_name", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                parameters.Add("op_count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateSuperUser, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                
                int userCount = parameters.Get<int>("op_count");                
                #endregion

                #region Response                
                if (userCount == 0)
                {
                    await sqlTransaction.CommitAsync();
                    response.ResultCount = parameters.Get<int>("op_user_id");
                    response.DomainName = parameters.Get<string>("op_domain_name");
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = UserMessage.UserCreated;
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
