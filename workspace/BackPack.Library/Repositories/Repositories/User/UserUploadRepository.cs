using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using Dapper;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class UserUploadRepository : GenericRepository, IUserUploadRepository
    {
        #region CreateUserUploadAsync
        public async Task<UserUploadQueryResponse> CreateUserUploadAsync(UserUploadQueryRequest request)
        {
            UserUploadQueryResponse response = new();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters                    
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                parameters.Add("ip_school_id", request.SchoolID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_first_name", request.FName, DbType.String);
                parameters.Add("ip_last_name", request.LName, DbType.String);
                parameters.Add("ip_email_id", request.EmailID, DbType.String);
                parameters.Add("ip_user_type", request.UserType, DbType.String);
                parameters.Add("ip_class_name", request.ClassName, DbType.String);
                parameters.Add("ip_gmail_id", request.GmailID, DbType.String);
                parameters.Add("ip_phone_number", request.PhoneNo, DbType.String);
                parameters.Add("ip_password_hash", request.Password, DbType.String);
                parameters.Add("ip_password_salt", request.PasswordSalt, DbType.String);
                parameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_user_id", DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_domain_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                #endregion

                var result = await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateBulkUploadUser, parameters, commandType: CommandType.StoredProcedure);
                int returnStatus = parameters.Get<int>("op_return_status");
                response.ReturnStatus = returnStatus;
                if (returnStatus == 1 && request.UserType?.ToLower().ToString() == "teacher")
                {
                    response.UserID = parameters.Get<int>("op_user_id");
                    response.DomainName = parameters.Get<string>("op_domain_name");
                }
                return response;
            }
            catch (Exception)
            {
                response.ReturnStatus = 0;
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
