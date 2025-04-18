using BackPack.Library.Constants;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Requests.Master.Group;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Group;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Master
{
    public class GroupRepository : GenericRepository, IGroupRepository
    {
        #region CreateGroupAsync        
        public async Task<BaseResponse> CreateGroupAsync(CreateGroupRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_group_name", request.GroupName, DbType.String);
                parameters.Add("ip_group_desc", request.GroupDesc, DbType.String);
                parameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                parameters.Add("ip_student_ids", string.Join(",", request.StudentsList!), DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogMessage.CreateGroup, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_out_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateGroup, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int StatusID = parameters.Get<int>("op_out_status");

                #region Response
                if (StatusID == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.CreateGroupSuccess
                    };
                }

                if (StatusID == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.DuplicateID,
                        Success = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        StatusMessage = MasterMessage.GroupDuplicate
                    };
                }

                if (StatusID == 3 || StatusID == 4)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.ExceptionMessage,
                        ExceptionMessage = (StatusID == 3) ? MasterMessage.GroupInvalidCourse : MasterMessage.GroupInvalidStudent
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ExceptionType = CommonMessage.ExceptionTypeFail,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
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

        #region UpdateGroupAsync
        public async Task<BaseResponse> UpdateGroupAsync(UpdateGroupRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_group_name", request.GroupName, DbType.String);
                parameters.Add("ip_group_desc", request.GroupDesc, DbType.String);
                parameters.Add("ip_group_id", request.GroupID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogConstant.UpdateGroup, DbType.String);
                parameters.Add("ip_student_ids", string.Join(",", request.StudentsList!), DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateGroup, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int StatusID = parameters.Get<int>("op_status");

                #region Response
                if (StatusID == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.UpdateGroupSuccess
                    };
                }

                if (StatusID == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.DuplicateID,
                        Success = true,
                        StatusCode = StatusCodes.Status409Conflict,
                        StatusMessage = MasterMessage.GroupDuplicate
                    };
                }

                if (StatusID == 3)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = true,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = MasterMessage.GroupDuplicate
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ExceptionType = CommonMessage.ExceptionTypeFail,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
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

        #region DeleteGroupAsync
        public async Task<BaseResponse> DeleteGroupAsync(DeleteGroupRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_group_id", request.GroupID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_activity_desc", LogConstant.DeleteGroup, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_return_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.DeleteGroup, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int StatusID = parameters.Get<int>("op_return_status_id");

                #region Response
                if (StatusID == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.DeleteGroupSuccess
                    };
                }

                if (StatusID == 2)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = true,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.ExceptionMessage
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ExceptionType = CommonMessage.ExceptionTypeFail,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
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

        #region EditGroupAsync
        public async Task<EditGroupResponse> EditGroupAsync(int GroupID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            EditGroupResponse response = new();
            EditGroupData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_group_id", GroupID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_student_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetGroup, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<EditGroupData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outStudentRecordSet = parameters.Get<string>("op_student_record_set");
                string studentRecordSetQuery = GlobalHelper.StringToString(outStudentRecordSet);
                var studentRecordSetData = await dbConnection.QueryAsync<StudentList>(studentRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                                
                #region Read db response
                var group = recordSetData.ToList();
                var groupStudent = studentRecordSetData.ToList();
                #endregion


                #region Response data
                if (group.Any())
                {
                    List<int> studentList = new();
                    responseData = group.First();

                    foreach (var student in groupStudent)
                    {
                        studentList.Add(student.StudentID);
                    }
                    responseData.StudentsList = studentList;
                }
                #endregion

                #region Response
                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
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

        #region ListGroupAsync
        public async Task<ListGroupResponse> ListGroupAsync(int CourseID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            ListGroupResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_course_id", CourseID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetGroups, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<ListGroupData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new ListGroupDataResult
                {
                    GetAllGroupsResult = recordSetData.ToList()
                };

                #region Response
                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
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
