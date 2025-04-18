using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Requests.School;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Responses.School;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Dependency.Library.Responses;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.School
{
    public class CreateSchoolRepository : GenericRepository, ICreateSchoolRepository
    {
        #region CreateSchool        
        public async Task<CreateSchoolResponse> CreateSchoolAsync(CreateSchoolRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CreateSchoolResponse response = new();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_school_name", request.Name, DbType.String);
                userParameters.Add("ip_school_desc", request.Desc, DbType.String);
                userParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateSchool, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_school_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                userParameters.Add("op_school_count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateSchool, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int Count = userParameters.Get<int>("op_school_count");
                #endregion

                #region Response 
                if (Count == 0)
                {
                    int SchoolID = userParameters.Get<int>("op_school_id");
                    var adminParameters = new DynamicParameters();
                    adminParameters.Add("ip_login_name", request.LoginName, DbType.String);
                    adminParameters.Add("ip_first_name", request.FirstName, DbType.String);
                    adminParameters.Add("ip_last_name", request.LastName, DbType.String);
                    adminParameters.Add("ip_email_id", request.EmailID, DbType.String);
                    adminParameters.Add("ip_phone_number", request.PhoneNo, DbType.String);
                    adminParameters.Add("ip_role", "SchoolAdmin", DbType.String);
                    adminParameters.Add("ip_is_active", false, DbType.Boolean);
                    adminParameters.Add("ip_is_primary", true, DbType.Boolean);
                    adminParameters.Add("ip_school_id", SchoolID, DbType.Int32);
                    adminParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                    adminParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    adminParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    adminParameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                    adminParameters.Add("op_user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    adminParameters.Add("op_domain_name", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                    adminParameters.Add("op_count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CreateSuperUser, adminParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int userCount = adminParameters.Get<int>("op_count");
                    response.ReturnStatus = userCount;

                    if (userCount == 0) 
                    {
                        await sqlTransaction.CommitAsync();
                        response.UserID = adminParameters.Get<int>("op_user_id");
                        response.DomainName = adminParameters.Get<string>("op_domain_name");
                    }
                    return response;

                }
                else
                {
                    if (Count > 0)
                    {
                        await sqlTransaction.RollbackAsync();
                        response.ReturnStatus = Count;
                        response.MessageID = CommonMessage.DuplicateID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = SchoolMessage.DuplicateSchool;
                    }
                    else
                    {
                        await sqlTransaction.RollbackAsync();
                        response.ReturnStatus = -1;
                        response.MessageID = CommonMessage.FailID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                    }
                }
                return response;
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                response.ReturnStatus = -1;
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
                response.ReturnStatus = -1;
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
