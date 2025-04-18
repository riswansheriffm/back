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
    public class CreateCourseRepository : GenericRepository, ICreateCourseRepository
    {
        #region CreateCourseAsync        
        public async Task<BaseResponse> CreateCourseAsync(CreateCourseRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();                
                userParameters.Add("ip_course_name", request.CourseName, DbType.String);
                userParameters.Add("ip_course_desc", request.CourseDesc, DbType.String);
                userParameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                userParameters.Add("ip_school_id", request.SchoolID, DbType.Int32);
                userParameters.Add("ip_course_type", request.CourseType, DbType.Int32);                
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateCourse, DbType.String);
                userParameters.Add("ip_image_url", request.ImageURL, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_course_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateCourse, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #endregion

                #region Response  
                int ReturnStatus = userParameters.Get<int>("op_course_id");

                if (ReturnStatus > 0)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.CreateCourse
                    };
                }

                if (ReturnStatus < 0)
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.DuplicateID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = MasterMessage.CourseCreationDuplicate
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
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
