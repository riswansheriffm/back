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
    public class UpdateCourseRepository : GenericRepository, IUpdateCourseRepository
    {
        #region UpdateCourseAsync        
        public async Task<BaseResponse> UpdateCourseAsync(UpdateCourseRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set user parameters   
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                userParameters.Add("ip_course_name", request.CourseName, DbType.String);
                userParameters.Add("ip_course_desc", request.CourseDesc, DbType.String);
                userParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.UpdateCourse, DbType.String);
                userParameters.Add("ip_image_url", request.ImageURL, DbType.String);
                userParameters.Add("ip_course_type", request.CourseType, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add(ProcedureConstant.OpReturnStatus, dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateCourse, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                #endregion

                #region Response 
                int returnStatus = userParameters.Get<int>(ProcedureConstant.OpReturnStatus);

                if (returnStatus == 0)
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = MasterMessage.UpdateCreationCourseFail,
                    };
                }
                #endregion

                #region Course with Teacher and Student mappings

                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                parameters.Add("ip_teacher_ids", string.Join(",", request.TeachersList!), DbType.String);
                parameters.Add("ip_student_ids", string.Join(",", request.StudentsList!), DbType.String);
                parameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.CreateCourseMapping, DbType.String);
                parameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpReturnStatus, dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CourseTeacherStudentMappings, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                returnStatus = userParameters.Get<int>(ProcedureConstant.OpReturnStatus);

                #region Response
                if (returnStatus > 0)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.UpdateCourse
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
                        StatusMessage = MasterMessage.UpdateCreationCourseFail,
                    };
                }
                #endregion

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
