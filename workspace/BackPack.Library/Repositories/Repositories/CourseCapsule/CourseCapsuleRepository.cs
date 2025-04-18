
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Responses.Master.Course;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class CourseCapsuleRepository : GenericRepository, ICourseCapsuleRepository
    {
        #region GetAllCourseCapsulesAsync          
        public async Task<AllCourseCapsulesResponse> GetAllCourseCapsulesAsync(int LoginID, int SubjectID, string AppType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpSubjectId, SubjectID, DbType.Int32);
                parameters.Add("ip_app_type", AppType, DbType.String);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllCourseCapsules, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllCourseCapsulesResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new AllCourseCapsulesResponseData
                {
                    GetAllCourseCapsulesResult = details.ToList()
                };

                #region Response
                AllCourseCapsulesResponse response = new()
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
                return new AllCourseCapsulesResponse
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
                return new AllCourseCapsulesResponse
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

        #region GetAllCourseCapsulesForASubjectAsync          
        public async Task<AllCourseCapsulesForASubjectResponse> GetAllCourseCapsulesForASubjectAsync(int LoginID, int SubjectID, string AppType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpSubjectId, SubjectID, DbType.Int32);
                parameters.Add("ip_app_type", AppType, DbType.String);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllCourseCapsulesForASubject, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllCourseCapsulesForASubjectResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new AllCourseCapsulesForASubjectResponseData
                {
                    GetAllCourseCapsulesForASubjectResult = details.ToList()
                };

                #region Response
                AllCourseCapsulesForASubjectResponse response = new()
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
                return new AllCourseCapsulesForASubjectResponse
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
                return new AllCourseCapsulesForASubjectResponse
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

        #region GetAllCourseCapsuleFoldersForACourseCapsuleAsync         
        public async Task<FoldersForACourseCapsuleResponse> GetAllCourseCapsuleFoldersForACourseCapsuleAsync(int CourseCapsuleID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpCourseCapsuleId, CourseCapsuleID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllCourseCapsuleFoldersForCourseCapsule, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllCourseCapsuleFoldersForACourseCapsuleResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new FoldersForACourseCapsuleResponseData
                {
                    GetAllCourseCapsuleFoldersForACourseCapsuleResult = details.ToList()
                };

                #region Response
                FoldersForACourseCapsuleResponse response = new()
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
                return new FoldersForACourseCapsuleResponse
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
                return new FoldersForACourseCapsuleResponse
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

        #region GetAllPublicCourseCapsuleByDomainAndSubjectAsync           
        public async Task<PublicCourseCapsuleByDomainResponse> GetAllPublicCourseCapsuleByDomainAndSubjectAsync(int DomainID, int SubjectID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpSubjectId, SubjectID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllPublicCourseCapsuleByDomainAndSubject, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<PublicCourseCapsuleByDomain>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new PublicCourseCapsuleByDomainResponseData
                {
                    GetAllPublicCourseCapsuleByDomainAndSubjectResult = details.ToList()
                };

                #region Response
                PublicCourseCapsuleByDomainResponse response = new()
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
                return new PublicCourseCapsuleByDomainResponse
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
                return new PublicCourseCapsuleByDomainResponse
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

        #region GetAllLPStudentLicensesByCourseCapsuleAsync          
        public async Task<LPStudentLicensesByCourseCapsuleResponse> GetAllLPStudentLicensesByCourseCapsuleAsync(int LoginID, int CourseID, int CourseCapsuleID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpCourseId, CourseID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpCourseCapsuleId, CourseCapsuleID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetAllLpStudentLicensesByCourseCapsule, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<LPStudentLicensesByCourse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new LPStudentLicensesByCourseCapsuleResponseData
                {
                    GetAllLPStudentLicensesByCourseCapsuleResult = details.ToList()
                };

                #region Response
                LPStudentLicensesByCourseCapsuleResponse response = new()
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
                return new LPStudentLicensesByCourseCapsuleResponse
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
                return new LPStudentLicensesByCourseCapsuleResponse
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

        #region GetAllLPCourseLicensesAsync          
        public async Task<LPCourseLicensesResponse> GetAllLPCourseLicensesAsync(int LoginID, int CourseID, int CourseCapsuleID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpCourseId, CourseID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpCourseCapsuleId, CourseCapsuleID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllLpCourseLicenses, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetAllLPCourseLicensesResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new LPCourseLicensesResponseData
                {
                    GetAllLPCourseLicensesResult = details.ToList()
                };

                #region Response
                LPCourseLicensesResponse response = new()
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
                return new LPCourseLicensesResponse
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
                return new LPCourseLicensesResponse
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

        #region GetCourseCapsuleForReorderAsync          
        public async Task<CourseCapsuleForReorderResponse> GetCourseCapsuleForRecorderAsync(int LoginID, int SubjectID, int CourseCapsuleID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpSubjectId, SubjectID, DbType.Int32);
                parameters.Add(ProcedureConstant.IpCourseCapsuleId, CourseCapsuleID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_course_capsule_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllReorderCourseCapsuleFolders, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<GetCourseCapsuleForReorderFolder>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outCourseCapsuleRecordSet = parameters.Get<string>("op_course_capsule_record_set");
                string courseCapsuleRecordSetQuery = GlobalHelper.StringToString(outCourseCapsuleRecordSet);
                var courseCapsuleRecordSetData = await dbConnection.QueryAsync<GetCourseCapsuleForReorderResult>(courseCapsuleRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                List<GetCourseCapsuleForReorderFolder> listCourseCapsuleForReorderFolder = [];
                foreach (var courseCapsuleFolder in recordSetData)
                {
                    List<GetCourseCapsuleForReorderPod> listCourseCapsuleForReorderPod = [];
                    GetCourseCapsuleForReorderFolder courseCapsuleForReorderFolder = new();
                    courseCapsuleForReorderFolder.CourseCapsuleFolderID = courseCapsuleFolder.CourseCapsuleFolderID;
                    courseCapsuleForReorderFolder.LessonName = courseCapsuleFolder.LessonName;
                    courseCapsuleForReorderFolder.LessonDesc = courseCapsuleFolder.LessonDesc;
                    courseCapsuleForReorderFolder.DisplayOrder = courseCapsuleFolder.DisplayOrder;

                    #region Set parameters
                    var podParameters = new DynamicParameters();
                    podParameters.Add("ip_course_capsule_folder_id", courseCapsuleFolder.CourseCapsuleFolderID, DbType.Int32);
                    podParameters.Add(ProcedureConstant.IpCourseCapsuleId, CourseCapsuleID, DbType.Int32);
                    podParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllReorderCourseCapsulePods, podParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    var outPodRecordSet = podParameters.Get<string>(ProcedureConstant.OpRecordSet);
                    string podRecordSetQuery = GlobalHelper.StringToString(outPodRecordSet);
                    var podRecordSetData = await dbConnection.QueryAsync<GetCourseCapsuleForReorderPod>(podRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                    foreach (var courseCapsulePod in podRecordSetData)
                    {
                        GetCourseCapsuleForReorderPod courseCapsuleForReorderPod = new();
                        courseCapsuleForReorderPod.CourseCapsuleLessonPodID = courseCapsulePod.CourseCapsuleLessonPodID;
                        courseCapsuleForReorderPod.LessonPodName = courseCapsulePod.LessonPodName;
                        courseCapsuleForReorderPod.LessonPodDesc = courseCapsulePod.LessonPodDesc;
                        courseCapsuleForReorderPod.LessonPodModified = courseCapsulePod.LessonPodModified;
                        courseCapsuleForReorderPod.DisplayOrder = courseCapsulePod.DisplayOrder;

                        #region Set activity parameters
                        var activityParameters = new DynamicParameters();
                        activityParameters.Add("ip_course_capsule_lessonpod_id", courseCapsulePod.CourseCapsuleLessonPodID, DbType.Int32);
                        activityParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        #endregion

                        await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllReorderCourseCapsulePodActivities, activityParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                        var outActivityRecordSet = activityParameters.Get<string>(ProcedureConstant.OpRecordSet);
                        string activityRecordSetQuery = GlobalHelper.StringToString(outActivityRecordSet);
                        var activityRecordSetData = await dbConnection.QueryAsync<GetCourseCapsuleForReorderActivity>(activityRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                        courseCapsuleForReorderPod.CapsulePodActivities = activityRecordSetData.ToList();
                        listCourseCapsuleForReorderPod.Add(courseCapsuleForReorderPod);
                    }
                    courseCapsuleForReorderFolder.CapsuleLessonPods = listCourseCapsuleForReorderPod;
                    listCourseCapsuleForReorderFolder.Add(courseCapsuleForReorderFolder);
                }
                var responseData = new CourseCapsuleForReorderResponseData();
                if (courseCapsuleRecordSetData.Any())
                {
                    responseData = new CourseCapsuleForReorderResponseData
                    {
                        GetCourseCapsuleForReorderResult = new GetCourseCapsuleForReorderResult
                        {
                            CourseCapsuleID = courseCapsuleRecordSetData.FirstOrDefault()!.CourseCapsuleID,
                            CourseCapsuleName = courseCapsuleRecordSetData.FirstOrDefault()!.CourseCapsuleName,
                            CourseCapsuleDesc = courseCapsuleRecordSetData.FirstOrDefault()!.CourseCapsuleDesc,
                            ImageURL = courseCapsuleRecordSetData.FirstOrDefault()!.ImageURL,
                            CapsuleFolders = listCourseCapsuleForReorderFolder
                        }
                    };
                }


                #region Response
                CourseCapsuleForReorderResponse response = new()
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
                return new CourseCapsuleForReorderResponse
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
                return new CourseCapsuleForReorderResponse
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
