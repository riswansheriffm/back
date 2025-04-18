using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Requests.School;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.School
{
    public class CreateSchoolBulkRepository : GenericRepository, ICreateSchoolBulkRepository
    {
        #region CreateBulkSchool       
        public async Task<BaseResponse> CreateSchoolBulkAsync(CreateSchoolBulkRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();
            try
            {
                #region Store users                
                for (int index = 0; index < request.SchoolList?.Count; index++)
                {
                    SchoolList school = request.SchoolList[index];

                    #region Set user parameters                    
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                    userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_role", "SchoolAdmin", DbType.String);
                    userParameters.Add("ip_school_name", school.SchoolName, DbType.String);
                    userParameters.Add("ip_school_desc", school.SchoolDescrption, DbType.String);
                    userParameters.Add("ip_login_name", school.LoginName, DbType.String);
                    userParameters.Add("ip_first_name", school.FirstName, DbType.String);
                    userParameters.Add("ip_last_name", school.LastName, DbType.String);
                    userParameters.Add("ip_email_id", school.EmailID, DbType.String);
                    userParameters.Add("ip_phone_no", school.PhoneNo, DbType.String);                    
                    userParameters.Add("ip_activity_desc", LogMessage.CreateSchool, DbType.String);
                    userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    userParameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);                    
                    userParameters.Add("op_user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    userParameters.Add("op_domain_name", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                    #endregion

                    var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateBulkUploadSchool, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int userReturnStatus = userParameters.Get<int>("op_return_status");

                    #region Response 
                    if (userReturnStatus == 1)
                    {
                        response.MessageID = CommonMessage.SuccessID;
                        response.Success = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        response.StatusMessage = SchoolMessage.SchoolCreated;
                    }
                    else if (userReturnStatus == 2)
                    {
                        response.MessageID = CommonMessage.DuplicateID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = SchoolMessage.DuplicateSchool;
                    }
                    else
                    {
                        response.MessageID = CommonMessage.FailID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                    }
                    #endregion
                }
                #endregion

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
