using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Master.Course;
using BackPack.Library.Requests.Master.Course;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Master.Course
{
    public class DeleteCourseRepository : GenericRepository, IDeleteCourseRepository
    {
        #region DeleteCourseAsync        
        public async Task<BaseResponse> DeleteCourseAsync(DeleteCourseRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set user parameters   
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.DeleteCourse, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_return_status", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.DeleteCourse, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                #endregion

                #region Response  
                int returnStatus = userParameters.Get<int>("op_return_status");
                if (returnStatus > 0)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.DeleteCourse
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
                        StatusMessage = MasterMessage.DeleteCreationCourseFail
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
