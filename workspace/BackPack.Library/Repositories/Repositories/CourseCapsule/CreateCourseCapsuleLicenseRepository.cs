using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class CreateCourseCapsuleLicenseRepository : GenericRepository, ICreateCourseCapsuleLicenseRepository
    {
        #region CreateCourseCapsuleLicenseAsync        
        public async Task<BaseResponse> CreateCourseCapsuleLicenseAsync(CreateCourseCapsuleLicenseRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            List<int> toStudentIds = [];

            try
            {
                List<int> studentIds = [];

                if (request.WhomToLicense!.Equals("all", StringComparison.CurrentCultureIgnoreCase))
                {
                    #region Set parameters                
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    userParameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                    userParameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                    userParameters.Add("ip_duration", request.Duration, DbType.Int32);
                    userParameters.Add("ip_start_date", request.StartDate, DbType.Date);
                    userParameters.Add("ip_license_action", request.LicenseAction, DbType.String);
                    userParameters.Add("ip_activity_desc", LogMessage.CreateCourseLicense, DbType.String);
                    userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    userParameters.Add("op_return_id", DbType.Int64, direction: ParameterDirection.Output);
                    userParameters.Add("op_publish_course_capsule_id", DbType.Int64, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_lp_course_license", userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int ReturnID = userParameters.Get<int>("op_return_id");
                    int PublishCourseCapsuleID = userParameters.Get<int>("op_publish_course_capsule_id");

                    if (ReturnID == 1 && PublishCourseCapsuleID == 0)
                    {
                        await sqlTransaction.RollbackAsync();
                        return new BaseResponse()
                        {
                            MessageID = CommonMessage.FailID,
                            Success = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            ExceptionType = CommonMessage.ExceptionTypeNormal,
                            StatusMessage = CourseCapsuleMessage.InvalidCourseCapsule
                        };
                    }

                    #region Set parameters  
                    var parameters = new DynamicParameters();
                    parameters.Add(ProcedureConstant.IpCourseId, request.CourseID, DbType.Int32);
                    parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + "get_non_license_student_for_course", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                    string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                    var recordSetData = await dbConnection.QueryAsync<int>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                    studentIds.AddRange(recordSetData);

                }

                if (request.WhomToLicense.Equals("student", StringComparison.CurrentCultureIgnoreCase))
                {
                    studentIds = toStudentIds;
                }

                if (studentIds.Count == 0)
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CourseCapsuleMessage.LicenseFailNoStudent
                    };
                }

                foreach (int studentId in studentIds)
                {
                    #region Set parameters                
                    var domainParameters = new DynamicParameters();
                    domainParameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                    domainParameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                    domainParameters.Add("ip_student_id", studentId, DbType.Int32);
                    domainParameters.Add("ip_duration", request.Duration, DbType.Int32);
                    domainParameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                    domainParameters.Add("ip_start_date", request.StartDate, DbType.Date);
                    domainParameters.Add("ip_license_action", request.LicenseAction, DbType.String);
                    domainParameters.Add("ip_license_type", request.LicenseType, DbType.String);
                    domainParameters.Add("ip_activity_desc", LogMessage.DistributeCourseCapsule, DbType.String);
                    domainParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    domainParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "distribute_course_capsule_license", domainParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                }

                await sqlTransaction.CommitAsync();
                return new BaseResponse
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = CourseCapsuleMessage.CreateCourseCapsuleLicense
                };
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
