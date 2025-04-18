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
    public class CRDistLessonPodSummaryTierReportRepository : GenericRepository, ICRDistLessonPodSummaryTierReportRepository
    {
        #region CRDistLessonPodSummaryTierReportAsync
        public async Task<CRDistLessonPodSummaryTierReportResponse> CRDistLessonPodSummaryTierReportAsync(int LoginID, int LessonUnitDistID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            CRDistLessonPodSummaryTierReportResponse response = new();
            CRDistLessonPodSummaryTierReportData responseData = new();
            List<CRDistLessonPodSummaryTierData> listTierData = new();

            #region Tier range
            List<TierRangeResponse> listTierRange = new();
            TierRangeResponse tierRange;

            tierRange = new()
            {
                TierName = "Tier 1",
                MinRange = 80,
                MaxRange = 100
            };
            listTierRange.Add(tierRange);

            tierRange = new()
            {
                TierName = "Tier 2",
                MinRange = 65,
                MaxRange = 79
            };
            listTierRange.Add(tierRange);

            tierRange = new()
            {
                TierName = "Tier 3",
                MinRange = 0,
                MaxRange = 64
            };
            listTierRange.Add(tierRange);
            #endregion

            try
            {
                foreach (var range in listTierRange)
                {
                    List<CRDistLessonPodSummaryTierStudentData> listTierStudentData = new();
                    #region Tier
                    CRDistLessonPodSummaryTierData tier = new()
                    {
                        TierName = range.TierName,
                        TierStartPercentage = range.MinRange,
                        TierEndPercentage = range.MaxRange
                    };
                    #endregion

                    #region Set parameters            
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_login_id", LoginID, DbType.Int32);
                    parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
                    parameters.Add("ip_min_range", range.MinRange, DbType.Int32);
                    parameters.Add("ip_max_range", range.MaxRange, DbType.Int32);
                    parameters.Add("op_result", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    var dbResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + "get_cr_dist_lesson_pod_tier_summary_report", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    var outRecordSet = parameters.Get<string>("op_result");
                    string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                    var recordSetData = await dbConnection.QueryAsync<CRDistLessonPodSummaryTierQueryData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                    #region Set response
                    if (recordSetData.Any())
                    {
                        foreach (var data in recordSetData)
                        {
                            CRDistLessonPodSummaryTierStudentData studentData = new()
                            {
                                AverageScore = data.AverageScore,
                                TimelyCompletion = data.AverageTimelyCompletion,
                                TimeSpent = data.AverageTimeSpent,
                                StudentName = data.FullName
                            };
                            listTierStudentData.Add(studentData);
                        }
                    }
                    tier.StudentList = listTierStudentData;
                    listTierData.Add(tier);
                    responseData.TierData = listTierData;
                    responseData.LessonUnitDistID = LessonUnitDistID;
                    #endregion
                }

                #region Response
                await sqlTransaction.CommitAsync();
                response.Data = new CRDistLessonPodSummaryTierReport
                {
                    GetCRDistLessonPodTierSummaryReportResult = responseData
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
