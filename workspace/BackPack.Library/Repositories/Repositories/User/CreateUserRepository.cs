using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using Dapper;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class CreateUserRepository : GenericRepository, ICreateUserRepository
    {
        #region CreateUser        
        public async Task<BaseResponse> CreateUserAsync(UserRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            BaseResponse response = new();
            var userPasswordSalt = Hash.GetSecureSalt();
            var userPasswordHash = Hash.HashUsingPbkdf2("1234", userPasswordSalt);

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();                
                userParameters.Add("ip_login_name", request.LoginName, DbType.String);
                userParameters.Add("ip_first_name", request.FName, DbType.String);
                userParameters.Add("ip_last_name", request.LName, DbType.String);
                userParameters.Add("ip_email_id", request.EmailID, DbType.String);
                userParameters.Add("ip_gmail_id", request.GmailID, DbType.String);
                userParameters.Add("ip_phone_no", request.PhoneNo, DbType.String);
                userParameters.Add("ip_domain_name", request.DomainName, DbType.String);
                userParameters.Add("ip_is_active", (request.ActiveFlag == 1), DbType.Boolean);
                userParameters.Add("ip_is_primary", (request.PrimaryFlag == 1), DbType.Boolean);
                userParameters.Add("ip_password", userPasswordHash, DbType.String);
                userParameters.Add("ip_password_salt", Convert.ToBase64String(userPasswordSalt).ToString(), DbType.String);
                userParameters.Add("ip_role", request.UserType, DbType.String);
                userParameters.Add("ip_school_id", request.SchoolID, DbType.Int32);
                userParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                userParameters.Add("op_user_id", DbType.Int32, direction: ParameterDirection.Output);
                userParameters.Add("op_domain_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateUser, userParameters, commandType: CommandType.StoredProcedure);
                int user_id = userParameters.Get<int>("op_user_id");
                response.ResultCount = user_id;
                response.DomainName = userParameters.Get<string>("op_domain_name");
                #endregion

                #region Response                
                if (user_id > 0)
                {
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = UserMessage.UserCreated;
                }
                else
                {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
} 
