using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class CRDistLessonPodTierStudentActivityReportRepository : GenericRepository, ICRDistLessonPodTierStudentActivityReportRepository
    {
        #region CRDistLessonPodTierStudentActivityReportAsync        
        public async Task<CRDistLessonPodTierStudentActivityReportResponse> CRDistLessonPodTierStudentActivityReportAsync(int LoginID, int LessonUnitDistID, string TierName, int MinRange, int MaxRange)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRDistLessonPodTierStudentActivityReportResponse response = new();
            CRDistLessonPodTierStudentActivityReportData responseData = new();
            List<CRDistLessonPodTierStudentActivityReportStudentData> listStudentData = new();
            List<CRDistLessonPodTierStudentActivityReportActivityData> listActivityData = new();
            CRDistLessonPodTierStudentActivityReportActivityData activityData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_min_range", MinRange, DbType.Int32);
                parameters.Add("ip_max_range", MaxRange, DbType.Int32);
                parameters.Add("op_result", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var dbResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + "get_cr_dist_lesson_pod_tier_student_activity_report", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                var outRecordSet = parameters.Get<string>("op_result");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<CRDistLessonPodTierStudentActivityReportQueryData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Set response
                if (recordSetData.Any())
                {
                    int CurContentID = 0;
                    int PreContentID = 0;

                    foreach (var data in recordSetData)
                    {
                        CurContentID = data.ContentID;
                        if (PreContentID == 0 || PreContentID != CurContentID)
                        {
                            if (PreContentID != CurContentID && PreContentID != 0)
                            {
                                activityData.StudentData = listStudentData;
                                listActivityData.Add(activityData);
                            }
                            PreContentID = CurContentID;
                            listStudentData = new();
                            activityData = new()
                            {
                                AppName = data.AppName,
                                ContentID = data.ContentID,
                                ControlName = data.ControlNames.Split(',').ToList()
                            };
                        }
                        CRDistLessonPodTierStudentActivityReportStudentData studentData = new()
                        {
                            ActivityStatus = data.ActivityStatus,
                            StudentID = data.StudentID,
                            StudentName = data.FullName,
                            StudentScore = string.IsNullOrWhiteSpace(data.StudentWork) ? new List<int>() : data.StudentWork.Split(',').Select(int.Parse).ToList()
                        };
                        listStudentData.Add(studentData);
                    }
                }
                responseData.LessonUnitDistID = LessonUnitDistID;
                responseData.TierName = TierName;
                activityData.StudentData = listStudentData;
                listActivityData.Add(activityData);
                responseData.ActivityDetails = listActivityData;
                #endregion

                #region Response
                await sqlTransaction.CommitAsync();
                response.Data = new CRDistLessonPodTierStudentActivityReport
                {
                    GetCRDistLessonPodTierStudentActivityReportResult = responseData
                };
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

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
