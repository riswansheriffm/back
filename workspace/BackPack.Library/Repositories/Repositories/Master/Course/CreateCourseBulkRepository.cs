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
    public class CreateCourseBulkRepository : GenericRepository, ICreateCourseBulkRepository
    {
        #region CreateBulkCourse       
        public async Task<BaseResponse> CreateCourseBulkAsync(CreateCourseBulkRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();

            try
            {
                int successCount = 0;
                int courseCount = 0;
                int subjectCount = 0;
                #region Store users                
                for (int index = 0; index < request.CourseList?.Count; index++)
                {

                    BulkCourseObject Course = request.CourseList[index];

                    #region Set user parameters                    
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_school_id", request.SchoolID, DbType.Int32);
                    userParameters.Add("ip_course_name", Course.CourseName, DbType.String);
                    userParameters.Add("ip_course_desc", Course.CourseDescrption, DbType.String);
                    userParameters.Add("ip_subject_name", Course.SubjectName, DbType.String);
                    userParameters.Add("ip_activity_desc", LogMessage.CreateCourse, DbType.String);
                    userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    userParameters.Add("op_return_status", DbType.Int64, direction: ParameterDirection.Output);
                    #endregion

                    var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateBulkUploadCourse, userParameters, commandType: CommandType.StoredProcedure);
                    int userReturnStatus = userParameters.Get<int>("op_return_status");
                    if (userReturnStatus == 1) successCount++;
                    if (userReturnStatus == 2)
                    {
                        subjectCount++;
                        break;
                    }
                    if (userReturnStatus == 3)
                    {
                        courseCount++;
                        break;
                    }
                }
                #endregion

                #region Response 
                if (successCount == request.CourseList?.Count)
                {
                    await sqlTransaction.CommitAsync();
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = MasterMessage.CreateBulkCourse;
                }
                else if (subjectCount > 0)
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.NotFoundID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = CommonMessage.BadRequestMessage;
                }
                else if (courseCount > 0)
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.DuplicateID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = MasterMessage.BulkCourseCreationDuplicate;
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                }
                #endregion

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
