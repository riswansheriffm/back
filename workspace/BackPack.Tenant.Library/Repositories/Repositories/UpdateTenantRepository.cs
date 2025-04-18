using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace BackPack.Tenant.Library.Repositories.Repositories
{
    public class UpdateTenantRepository(IConfiguration configuration) : GenericRepository(configuration), IUpdateTenantRepository
    {
        #region CreateTenantAsync
        public async Task<BaseResponse> UpdateTenantAsync(UpdateTenantRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_tenant_id", request.TenantID, DbType.Guid);
                parameters.Add("ip_name", request.Name, DbType.String);
                parameters.Add("ip_desc", request.Desc, DbType.String);
                parameters.Add("ip_street_address", request.StreetAddress, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_city", request.City, DbType.String);
                parameters.Add("ip_state", request.State, DbType.String);
                parameters.Add("ip_zipcode", request.ZipCode, DbType.String);
                parameters.Add("ip_max_students", request.MaxStudents, DbType.Int32);
                parameters.Add("ip_max_teachers", request.MaxTeachers, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.UpdateTenant, DbType.String);
                parameters.Add("ip_application_id", request.ApplicationID, DbType.String);
                parameters.Add("ip_access_token", request.AccessToken, DbType.String);
                parameters.Add("ip_source_id", request.SourceID, DbType.String);
                parameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateTenant, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int TenantStatus = parameters.Get<int>("op_status_id");

                if (TenantStatus == 0)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        StatusMessage = TenantMessage.CreateTenantExist
                    };
                }

                #region Response
                await sqlTransaction.CommitAsync();

                return new BaseResponse()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = TenantMessage.CreateTenantSuccess
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
