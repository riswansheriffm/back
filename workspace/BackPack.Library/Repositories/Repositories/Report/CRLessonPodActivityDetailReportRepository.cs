using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Report;
using BackPack.Library.Responses.Report;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Report
{
    public class CRLessonPodActivityDetailReportRepository : GenericRepository, ICRLessonPodActivityDetailReportRepository
    {
        #region CRLessonPodActivityDetailReportAsync
        public async Task<CRLessonPodActivityDetailReportResponse> CRLessonPodActivityDetailReportAsync(int LoginID, int ContentID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            CRLessonPodActivityDetailReportResponse response = new();
            CRLessonPodActivityDetailReportData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("LoginID", LoginID, DbType.Int32);
                parameters.Add("ContentID", ContentID, DbType.Int32);
                #endregion

                var dbResponse = await Task.FromResult(dbConnection!.QueryMultiple(ServiceConstant.SchemaLessonpods + "GetCRLessonPodActivityDetailReport", parameters, commandType: CommandType.StoredProcedure));

                #region Read db response
                var activityCount = dbResponse!.Read<DBQueryDataCountResponse>().ToList();

                if (activityCount[0].ActivityCount == 0)
                {
                    #region Response
                    response.Data = responseData;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.StatusMessage = CommonMessage.ReadMessage;
                    #endregion

                    return response;
                }
                var activityType = dbResponse!.Read<CRLessonPodActivityDetailReportQueryData>().ToList();
                var listControl = dbResponse!.Read<CRLessonPodActivityDetailReportQueryData>().ToList();
                var listStudentActivity = dbResponse!.Read<CRLessonPodActivityDetailReportStudentQueryData>().ToList();
                #endregion

                #region Set response data structure
                if (activityType.Any())
                {
                    var activityTypeData = activityType.First();

                    responseData.AppName = activityTypeData.AppName;
                }
                List<string> listControlName = new();
                if (listControl.Any())
                {
                    foreach (var controlData in listControl)
                    {
                        listControlName.Add(controlData.ControlName);
                    }
                    responseData.ControlName = listControlName;
                }

                List<CRLessonPodActivityDetailReportStudentData> listStudentData = new();
                if (listStudentActivity.Any())
                {
                    foreach (var studentData in listStudentActivity)
                    {
                        CRLessonPodActivityDetailReportStudentData activity = new()
                        {
                            StudentID = studentData.StudentID,
                            ActivityStatus = studentData.ActivityStatus,
                            StudentName = studentData.FullName,
                            StudentScore = studentData.StudentWork.Split(',').Select(float.Parse).ToList()
                        };
                        listStudentData.Add(activity);
                    }
                }

                responseData.StudentData = listStudentData;
                responseData.ContentID = ContentID;
                #endregion

                #region Response
                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                return response;
            }
            catch (NpgsqlException ex)
            {
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeSqlException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
            }
            catch (Exception ex)
            {
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
