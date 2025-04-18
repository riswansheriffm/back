using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class SaveCourseCapsuleRepository : GenericRepository, ISaveCourseCapsuleRepository
    {
        #region SaveCourseCapsuleAsync        
        public async Task<SaveCourseCapsuleResponse> SaveCourseCapsuleAsync(SaveCourseCapsuleRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                parameters.Add("ip_course_capsule_name", request.CourseCapsuleName, DbType.String);
                parameters.Add("ip_course_capsule_desc", request.CourseCapsuleDesc, DbType.String);
                parameters.Add("ip_image_url", request.ImageURL, DbType.String);
                parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                parameters.Add("ip_app_type", request.AppType, DbType.String);
                parameters.Add("ip_activity_desc", LogMessage.CreateCourseCapsule, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_course_capsule_id", DbType.Int64, direction: ParameterDirection.Output);
                #endregion

                var data = await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveCourseCapsule, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int courseCapsuleCounts = parameters.Get<int>("op_course_capsule_id");
                if (courseCapsuleCounts > 0)
                {
                    SaveCourseCapsuleResponse response = new()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = CourseCapsuleMessage.SaveCourseCapsule,
                        CourseCapsuleID = courseCapsuleCounts
                    };
                    await sqlTransaction.CommitAsync();
                    return response;
                }
                else
                {
                    SaveCourseCapsuleResponse response = new()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
                    };
                    await sqlTransaction.RollbackAsync();
                    return response;
                }
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new SaveCourseCapsuleResponse
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
                return new SaveCourseCapsuleResponse
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
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
