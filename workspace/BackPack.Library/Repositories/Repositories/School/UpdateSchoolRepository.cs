using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Requests.School;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.School
{
    public class UpdateSchoolRepository : GenericRepository, IUpdateSchoolRepository
    {
        #region UpdateSchool        
        public async Task<BaseResponse> UpdateSchoolAsync(UpdateSchoolRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_school_id", request.ID, DbType.Int32);
                userParameters.Add("ip_school_name", request.Name, DbType.String);
                userParameters.Add("ip_school_desc", request.Desc, DbType.String);
                userParameters.Add("ip_district_id", request.DomainID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.UpdateSchool, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add(ProcedureConstant.OpReturnStatusId, dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateSchool, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int ReturnStatusID = userParameters.Get<int>(ProcedureConstant.OpReturnStatusId);
                #endregion

                #region Response                
                if (ReturnStatusID == 1)
                {
                    var adminParameters = new DynamicParameters();
                    adminParameters.Add("ip_login_name", request.LoginName, DbType.String);
                    adminParameters.Add("ip_first_name", request.FirstName, DbType.String);
                    adminParameters.Add("ip_last_name", request.LastName, DbType.String);
                    adminParameters.Add("ip_email_id", request.EmailID, DbType.String);
                    adminParameters.Add("ip_phone_no", request.PhoneNo, DbType.String);
                    adminParameters.Add("ip_gmail_id", "", DbType.String);
                    adminParameters.Add("ip_is_primary", true, DbType.Boolean);
                    adminParameters.Add("ip_role", "SchoolAdmin", DbType.String);                    
                    adminParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    adminParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    adminParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    adminParameters.Add("ip_activity_desc", LogMessage.UpdateUser, DbType.String);
                    adminParameters.Add(ProcedureConstant.OpReturnStatusId, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.UpdateSuperUser, adminParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int UserReturnStatusID = adminParameters.Get<int>(ProcedureConstant.OpReturnStatusId);

                    if (UserReturnStatusID > 0)
                    {
                        await sqlTransaction.CommitAsync();
                        return new BaseResponse()
                        {
                            MessageID = CommonMessage.SuccessID,
                            Success = true,
                            StatusCode = StatusCodes.Status201Created,
                            StatusMessage = SchoolMessage.SchoolUpdate
                        };
                    }
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.DuplicateID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
                    };
                }

                return response;
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
