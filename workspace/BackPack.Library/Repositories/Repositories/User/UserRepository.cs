using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.User;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.User
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        #region ResetPasswordAsync
        public async Task<BaseResponse> ActivateUserAccountAsync(ResetPasswordRequest request, string Password, string PasswordSalt)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_domain_name", request.DistrictName, DbType.String);
                parameters.Add("ip_password", Password, DbType.String);
                parameters.Add("ip_password_salt", PasswordSalt, DbType.String);
                parameters.Add("op_return_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                var result = await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.ActivateUserAccount, parameters, commandType: CommandType.StoredProcedure);

                int queryStatus = parameters.Get<int>("op_return_status");

                return new BaseResponse()
                {
                    Success = true,
                    ResultCount = queryStatus
                };
            }
            catch (NpgsqlException ex)
            {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region ResetPasswordAsync
        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request, string Password, string PasswordSalt)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_name", request.LoginName, DbType.String);
                parameters.Add("ip_domain_name", request.DistrictName, DbType.String);
                parameters.Add("ip_password", Password, DbType.String);
                parameters.Add("ip_password_salt", PasswordSalt, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.ResetPassword, DbType.String);
                parameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.ResetPassword, parameters, commandType: CommandType.StoredProcedure);
                int result = parameters.Get<int>("op_status_id");

                return new BaseResponse()
                {
                    Success = true,
                    ResultCount = result
                };
            }
            catch (NpgsqlException ex)
            {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region UserAsync        
        public async Task<UserResponse> UserAsync(int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetUserById, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetUserResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var user = recordSetData;

                var responseData = new UserResponseData
                {
                    GetUserResult = user.FirstOrDefault()!
                };

                #region Response
                UserResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new UserResponse
                {
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
                return new UserResponse
                {
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

        #region AllUsersAsync        
        public async Task<AllUserResponse> AllUsersAsync(int DomainID, int SchoolID, string RoleName)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add("ip_role_name", RoleName, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetAllUsers, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);

                if (recordSetQuery == "")
                {
                    await sqlTransaction.RollbackAsync();
                    return new AllUserResponse
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
                    };
                }

                var recordSetData = await dbConnection.QueryAsync<GetAllUsersResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var user = recordSetData;

                var responseData = new AllUsersResponseData
                {
                    GetAllUsersResults = user.ToList()
                };

                #region Response
                AllUserResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllUserResponse
                {
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
                return new AllUserResponse
                {
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

        #region AllDomainUserAsync        
        public async Task<AllDomainUsersResponse> AllDomainUserAsync(int DomainID, int SchoolID, string Role)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add("ip_role", Role, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetAllDomainUsers, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllDomainUsersResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new AllDomainUsersResponseData
                {
                    GetAllDomainUsersResults = recordSetData.ToList()
                };

                #region Response
                AllDomainUsersResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllDomainUsersResponse
                {
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
                return new AllDomainUsersResponse
                {
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

        #region AllTeacherByClassAsync        
        public async Task<AllTeacherByClassResponse> AllTeacherByClassAsync(int LoginID, int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_course_id", CourseID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetAllTeachersByClass, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllTeachersByClassResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var teacher = recordSetData;

                var responseData = new AllTeacherByClassResponseData
                {
                    GetAllTeachersByClassResult = teacher.ToList()
                };

                #region Response
                AllTeacherByClassResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllTeacherByClassResponse
                {
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
                return new AllTeacherByClassResponse
                {
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

        #region SuperCaAsync        
        public async Task<SuperCaResponse> SuperCaAsync(int domainId)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", domainId, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.GetSuperCa, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SuperCaData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var user = recordSetData;

                var responseData = new SuperCaResult
                {
                    GetSuperCaResult = user.FirstOrDefault()!
                };

                #region Response
                SuperCaResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new SuperCaResponse
                {
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
                return new SuperCaResponse
                {
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
