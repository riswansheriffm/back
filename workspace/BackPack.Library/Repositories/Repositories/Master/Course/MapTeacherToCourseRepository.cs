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
    public class MapTeacherToCourseRepository : GenericRepository, IMapTeacherToCourseRepository
    {
        #region MapTeacherToCourseAsync       
        public async Task<BaseResponse> MapTeacherToCourseAsync(BulkMapTeacherToCourseRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            BaseResponse response = new();

            try
            {

                #region Store users                
                for (int index = 0; index < request.ListTeacherToCourse?.Count; index++)
                {

                    TeacherToCourse Course = request.ListTeacherToCourse[index];

                    #region Set user parameters                    
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    userParameters.Add("ip_course_name", Course.CourseName, DbType.String);
                    userParameters.Add("ip_login_name", Course.LoginName, DbType.String);
                    userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    userParameters.Add("ip_activity_desc", LogMessage.CreateCourseMapping, DbType.String);
                    userParameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + ProcedureConstant.MapTeacherToCourse, userParameters, commandType: CommandType.StoredProcedure);
                    int userReturnStatus = userParameters.Get<int>("op_return_status");

                    #region Response 
                    if (userReturnStatus == 1)
                    {
                        response.MessageID = CommonMessage.SuccessID;
                        response.Success = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        response.StatusMessage = MasterMessage.MapBulkCourse;
                    }
                    else if (userReturnStatus == 2)
                    {
                        response.MessageID = CommonMessage.DuplicateID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = MasterMessage.MapBulkCourseCreationDuplicate;
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
                return response;
            }
            catch (NpgsqlException ex)
            {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
