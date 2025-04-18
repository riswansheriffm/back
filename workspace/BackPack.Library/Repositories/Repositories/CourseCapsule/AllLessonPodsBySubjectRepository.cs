
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Responses.District;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class AllLessonPodsBySubjectRepository : GenericRepository, IAllLessonPodsBySubjectRepository
    {
        #region AllLessonPodsBySubjectAsync
        public async Task<AllLessonPodsBySubjectResponse> AllLessonPodsBySubjectAsync(int loginId, int subjectId)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllLessonPodsBySubjectResponse response = new();
            List<AllLessonPodsBySubjectData> listLessonPodsBySubjectData = [];

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpSubjectId, subjectId, DbType.Int32);
                parameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + ProcedureConstant.GetAllLessonsForASubject, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<AllLessonBySubject>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                
                for ( int index = 0; index < recordSetData.ToList().Count; index++)
                {
                    List<AllLessonPodsBySubjectLessonpod> listAllLessonPods = [];
                    AllLessonPodsBySubjectData allLessonPodsBySubjectData = new()
                    {
                        LessonID = recordSetData.ToList()[index].LessonID,
                        LessonName = recordSetData.ToList()[index].LessonName,
                        LessonDesc = recordSetData.ToList()[index].LessonDesc,
                        DisplayOrder = recordSetData.ToList()[index].DisplayOrder
                    };

                    #region Set Lessonpod parameter
                    var lessonpodParameters = new DynamicParameters();
                    lessonpodParameters.Add(ProcedureConstant.IpLessonId, recordSetData.ToList()[index].LessonID, DbType.Int32);
                    lessonpodParameters.Add(ProcedureConstant.IpAuthorId, loginId, DbType.Int32);
                    lessonpodParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllMyLessonUnitByLesson, lessonpodParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    var outLessonpodRecordSet = lessonpodParameters.Get<string>(ProcedureConstant.OpRecordSet);
                    string lessonpodRecordSetQuery = GlobalHelper.StringToString(outLessonpodRecordSet);
                    var lessonpodRecordSetData = await dbConnection.QueryAsync<AllLessonByResultLessonpodQuery>(lessonpodRecordSetQuery, sqlTransaction, commandType: CommandType.Text);
                    for ( int lessonpodIndex = 0; lessonpodIndex < lessonpodRecordSetData.ToList().Count; lessonpodIndex++)
                    {
                        AllLessonPodsBySubjectLessonpod allLessonPodsBySubjectLessonpod = new()
                        {
                            LessonUnitID = lessonpodRecordSetData.ToList()[lessonpodIndex].LessonUnitID,
                            LessonPodName = lessonpodRecordSetData.ToList()[lessonpodIndex].LessonName,
                            LessonPodDesc = lessonpodRecordSetData.ToList()[lessonpodIndex].LessonDesc,
                            AccessType = lessonpodRecordSetData.ToList()[lessonpodIndex].AccessType,
                            Author = lessonpodRecordSetData.ToList()[lessonpodIndex].FullName,
                            AuthorID = lessonpodRecordSetData.ToList()[lessonpodIndex].AuthorID,
                            LessonPodVersion = lessonpodRecordSetData.ToList()[lessonpodIndex].LessonPodVersion
                        };

                        #region MyRegion
                        var activityParameters = new DynamicParameters();
                        activityParameters.Add(ProcedureConstant.IpLessonUnitId, lessonpodRecordSetData.ToList()[lessonpodIndex].LessonUnitID);
                        activityParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                        #endregion

                        await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllActivitiesByLessonpod, activityParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                        var outActivityRecordSet = activityParameters.Get<string>(ProcedureConstant.OpRecordSet);
                        string activityRecordSetQuery = GlobalHelper.StringToString(outActivityRecordSet);
                        var activityRecordSetData = await dbConnection.QueryAsync<AllLessonPodsBySubjectActivity>(activityRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                        allLessonPodsBySubjectLessonpod.PodActivities = activityRecordSetData.ToList();
                        allLessonPodsBySubjectLessonpod.TotalActivities = activityRecordSetData.ToList().Count();
                        listAllLessonPods.Add(allLessonPodsBySubjectLessonpod);
                    }
                    allLessonPodsBySubjectData.LessonPods = listAllLessonPods;
                    listLessonPodsBySubjectData.Add(allLessonPodsBySubjectData);
                }

                AllLessonPodsBySubjectResult responseData = new()
                {
                    GetAllLessonPodsBySubjectResult = listLessonPodsBySubjectData
                };

                response.Data = responseData;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllLessonPodsBySubjectResponse
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
                return new AllLessonPodsBySubjectResponse
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
