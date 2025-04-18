using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Requests.User;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class UpdateStudentRepository : GenericRepository, IUpdateStudentRepository
    {
        #region UpdateStudentAsync
        public async Task<BaseResponse> UpdateStudentAsync(UpdateStudentRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_first_name", request.FName, DbType.String);
                parameters.Add("ip_last_name", request.LName, DbType.String);
                parameters.Add("ip_gmail_id", request.GmailID, DbType.String);
                parameters.Add("ip_email_id", request.EmailID, DbType.String);                
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.UpdateStudent, DbType.String);
                parameters.Add("op_return_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                var result = await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.UpdateStudent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int ReturnStatusID = parameters.Get<int>("op_return_status_id");

                #region Response
                if (ReturnStatusID > 0)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = UserMessage.UpdateStudent
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest, 
                        StatusMessage = UserMessage.UpdateStudentDuplicate
                    };
                }
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
