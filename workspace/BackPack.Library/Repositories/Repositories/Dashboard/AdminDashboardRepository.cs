using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Dashboard;
using Microsoft.AspNetCore.Http;
using System.Data;
using Dapper;
using BackPack.Library.Responses.User;
using BackPack.Library.Constants;
using BackPack.MessageContract.Library;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Dashboard
{
    public class AdminDashboardRepository : GenericRepository, IAdminDashboardRepository
    {
        #region SuperAdminDashboardAsync        
        public async Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add(ProcedureConstant.OpActivityLogRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_domain_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.SuperAdminDashboard, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetSuperAdminDashboardResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outActivityLogRecordSet = parameters.Get<string>(ProcedureConstant.OpActivityLogRecordSet);
                string activityLogRecordSetQuery = GlobalHelper.StringToString(outActivityLogRecordSet);
                var activityLogRecordSetData = await dbConnection.QueryAsync<TenantActivityList>(activityLogRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outDomainRecordSet = parameters.Get<string>("op_domain_record_set");
                string domainRecordSetQuery = GlobalHelper.StringToString(outDomainRecordSet);
                var domainRecordSetData = await dbConnection.QueryAsync<TenantDistrictSummary>(domainRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var admin = recordSetData;
                var activity = activityLogRecordSetData;
                var districtsummary = domainRecordSetData;

                var responseData = new SuperAdminDashboardResponseData
                {
                    GetSuperAdminDashboardResults = new GetSuperAdminDashboardResult
                    {
                        TotalDistricts = admin.First().TotalDistricts,
                        TotalSchools = admin.First().TotalSchools,
                        TotalStudents = admin.First().TotalStudents,
                        TotalCourses = admin.First().TotalCourses,
                        TotalContents = admin.First().TotalContents,
                        TotalTeachers = admin.First().TotalTeachers,
                        ActivityList = activity.ToList(),
                        DistrictSummary = districtsummary.ToList()
                    }
                };

                #region Response
                GetSuperAdminDashboardAcceptedEvent response = new()
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
                return new GetSuperAdminDashboardAcceptedEvent
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
                return new GetSuperAdminDashboardAcceptedEvent
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

        #region DistrictAdminDashboardAsync        
        public async Task<DistrictAdminDashboardResponse> DistrictAdminDashboardAsync(int DomainID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);                
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add(ProcedureConstant.OpActivityLogRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_school_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.DistrictAdminDashboard, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetDistrictAdminDashBoardResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outActivityLogRecordSet = parameters.Get<string>(ProcedureConstant.OpActivityLogRecordSet);
                string activityLogRecordSetQuery = GlobalHelper.StringToString(outActivityLogRecordSet);
                var activityLogRecordSetData = await dbConnection.QueryAsync<ActivityList>(activityLogRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSchoolRecordSet = parameters.Get<string>("op_school_record_set");
                string schoolRecordSetQuery = GlobalHelper.StringToString(outSchoolRecordSet);
                var schoolRecordSetData = await dbConnection.QueryAsync<SchoolSummary>(schoolRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var admin = recordSetData;
                var activity = activityLogRecordSetData;
                var schoolsummary = schoolRecordSetData;

                var responseData = new DistrictAdminDashboardResponseData
                {
                    GetDistrictAdminDashBoardResults = new GetDistrictAdminDashBoardResult
                    {
                        TotalAssignments = admin.First().TotalAssignments,
                        TotalSchools = admin.First().TotalSchools,
                        TotalStudents = admin.First().TotalStudents,
                        TotalContents = admin.First().TotalContents,
                        TotalCourses = admin.First().TotalCourses,
                        TotalTeachers = admin.First().TotalTeachers,
                        ActivityList = activity.ToList(),
                        SchoolList = schoolsummary.ToList()
                    }
                };

                #region Response
                DistrictAdminDashboardResponse response = new()
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
                return new DistrictAdminDashboardResponse
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
                return new DistrictAdminDashboardResponse
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

        #region CurriculumAdminDashboardAsync        
        public async Task<CurriculumAdminDashBoardResponse> CurriculumAdminDashboardAsync(int DomainID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add(ProcedureConstant.OpActivityLogRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_subject_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.CurriculumAdminDashboard, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetCurriculumAdminDashBoardResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outActivityLogRecordSet = parameters.Get<string>(ProcedureConstant.OpActivityLogRecordSet);
                string activityLogRecordSetQuery = GlobalHelper.StringToString(outActivityLogRecordSet);
                var activityLogRecordSetData = await dbConnection.QueryAsync<ActivityList>(activityLogRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outSubjectRecordSet = parameters.Get<string>("op_subject_record_set");
                string subjectRecordSetQuery = GlobalHelper.StringToString(outSubjectRecordSet);
                var subjectRecordSetData = await dbConnection.QueryAsync<SubjectList>(subjectRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var admin = recordSetData;
                var activity = activityLogRecordSetData;
                var subjectsummary = subjectRecordSetData;

                var responseData = new CurriculumAdminDashBoardResponseData
                {
                    GetCurriculumAdminDashBoardResults = new GetCurriculumAdminDashBoardResult
                    {
                        TotalSubjects = admin.First().TotalSubjects,
                        TotalPrivateContent = admin.First().TotalPrivateContent,
                        TotalPublicContent = admin.First().TotalPublicContent,
                        ActivityList = activity.ToList(),
                        SubjectList = subjectsummary.ToList()
                    }
                };

                #region Response
                CurriculumAdminDashBoardResponse response = new()
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
                return new CurriculumAdminDashBoardResponse
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
                return new CurriculumAdminDashBoardResponse
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

        #region SchoolAdminDashboardAsync        
        public async Task<SchoolAdminDashboardResponse> SchoolAdminDashboardAsync(int SchoolID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add(ProcedureConstant.OpActivityLogRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_course_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + ProcedureConstant.SchoolAdminDashboard, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetSchoolAdminDashboardResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outActivityLogRecordSet = parameters.Get<string>(ProcedureConstant.OpActivityLogRecordSet);
                string activityLogRecordSetQuery = GlobalHelper.StringToString(outActivityLogRecordSet);
                var activityLogRecordSetData = await dbConnection.QueryAsync<ActivityList>(activityLogRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outCourseRecordSet = parameters.Get<string>("op_course_record_set");
                string courseRecordSetQuery = GlobalHelper.StringToString(outCourseRecordSet);
                var courseRecordSetData = await dbConnection.QueryAsync<CourseList>(courseRecordSetQuery, sqlTransaction, commandType: CommandType.Text);


                var admin = recordSetData;
                var activity = activityLogRecordSetData;
                var coursesummary = courseRecordSetData;

                var responseData = new SchoolAdminDashboardResponseData
                {
                    GetSchoolAdminDashboardResult = new GetSchoolAdminDashboardResult
                    {
                        TotalTeachers = admin.First().TotalTeachers,
                        TotalStudents = admin.First().TotalStudents,
                        TotalCourses = admin.First().TotalCourses,
                        ActivityList = activity.ToList(),
                        CourseList = coursesummary.ToList()
                    }
                };

                #region Response
                SchoolAdminDashboardResponse response = new()
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
                return new SchoolAdminDashboardResponse
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
                return new SchoolAdminDashboardResponse
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
