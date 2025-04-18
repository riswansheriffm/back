using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.District;
using BackPack.Library.Requests.District;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Responses.School;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.District
{
    public class CreateDistrictRepository : GenericRepository, ICreateDistrictRepository
    {
        #region CreateDistrict        
        public async Task<CreateSchoolResponse> CreateDistrictAsync(CreateDistrictRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CreateSchoolResponse response = new();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_activity_by", request.ActivityBy, DbType.Int32);
                userParameters.Add("ip_domain_name", request.Name, DbType.String);
                userParameters.Add("ip_domain_desc", request.Desc, DbType.String);
                userParameters.Add("ip_city", request.City, DbType.String);
                userParameters.Add("ip_street_address", request.StreetAddress, DbType.String);
                userParameters.Add("ip_state", request.State, DbType.String);
                userParameters.Add("ip_zipcode", request.ZipCode, DbType.String);
                userParameters.Add("ip_access_type", request.AccessType, DbType.String);
                userParameters.Add("ip_max_students", request.MaxStudents, DbType.Int32);
                userParameters.Add("ip_max_teachers", request.MaxTeachers, DbType.Int32);
                userParameters.Add("ip_tenant_id", request.TenantID, DbType.Guid);
                userParameters.Add("ip_user_type_id", 3, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateDomain, DbType.String);
                userParameters.Add("ip_application_id", request.ApplicationID, DbType.String);
                userParameters.Add("ip_access_token", request.AccessToken, DbType.String);                
                userParameters.Add("ip_source_id", request.SourceID, DbType.String);
                userParameters.Add("op_count", dbType: DbType.Int32, direction: ParameterDirection.Output);
                userParameters.Add("op_domain_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var userResult = await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + "create_domain", userParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                int Count = userParameters.Get<int>("op_count");
                int DistrictID = userParameters.Get<int>("op_domain_id");
                response.DomainID = DistrictID;
                #endregion 

                #region Response 
                if (DistrictID > 0)
                {
                    var adminParameters = new DynamicParameters();
                    adminParameters.Add("ip_login_name", request.LoginName, DbType.String);
                    adminParameters.Add("ip_first_name", request.FirstName, DbType.String);
                    adminParameters.Add("ip_last_name", request.LastName, DbType.String);
                    adminParameters.Add("ip_email_id", request.EmailID, DbType.String);
                    adminParameters.Add("ip_phone_number", request.PhoneNo, DbType.String);
                    adminParameters.Add("ip_role", "DistrictAdmin", DbType.String);
                    adminParameters.Add("ip_is_active", false, DbType.Boolean);
                    adminParameters.Add("ip_is_primary", true, DbType.Boolean);
                    adminParameters.Add("ip_school_id", 0, DbType.Int32);
                    adminParameters.Add("ip_domain_id", DistrictID, DbType.Int32);
                    adminParameters.Add("ip_activity_by", request.ActivityBy, DbType.Int32);
                    adminParameters.Add("ip_user_type_id", 3, DbType.Int32);
                    adminParameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                    userParameters.Add("ip_application_id", request.ApplicationID, DbType.String);
                    userParameters.Add("ip_access_token", request.AccessToken, DbType.String);
                    userParameters.Add("ip_source_id", request.SourceID, DbType.String);
                    adminParameters.Add("op_user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    adminParameters.Add("op_domain_name", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                    adminParameters.Add("op_count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var Result = await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + "create_super_user", adminParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int UserID = adminParameters.Get<int>("op_user_id");
                    int userCount = adminParameters.Get<int>("op_count");
                    string DomainName = adminParameters.Get<string>("op_domain_name");

                    response.ReturnStatus = userCount;
                    if (userCount == 0)
                    {                        
                        response.UserID = adminParameters.Get<int>("op_user_id");
                        response.DomainName = adminParameters.Get<string>("op_domain_name");
                        await sqlTransaction.CommitAsync();
                    };
                }
                else
                {
                    if (Count > 0)
                    {
                        response.MessageID = CommonMessage.DuplicateID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = DistrictMessage.DuplicateDistrcit;
                        response.ReturnStatus = 1;
                    }
                    else
                    {
                        response.MessageID = CommonMessage.FailID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                        response.ReturnStatus = 1;
                    }
                    await sqlTransaction.RollbackAsync();
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
                response.ReturnStatus = 1;
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
                response.ReturnStatus = 1;
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
