using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Helpers.Common;
using BackPack.Library.Helpers.LessonPod;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Activity;
using BackPack.Library.Requests.Activity;
using BackPack.Library.Responses.Activity;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.Activity
{
    public class ActivitySaveRepository : GenericRepository, IActivitySaveRepository
    {
        #region SaveStudentActivityAsync
        public async Task<BaseResponse> SaveStudentActivityAsync(StudentActivityRequest request)
        {
            var response = new BaseResponse();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Save Activity
                List<StudentOtherActivity> studentOtherActivityList = [];
                int activityCount = request.OtherActivity.Count;
                if (activityCount == 0)
                {
                    StudentOtherActivity studentOtherActivity = new()
                    {
                        ContentID = request.ContentID,
                        Attempts = request.Attempts,
                        Reset = request.Reset,
                        Offline = request.Offline,
                        DownloadVersion = request.DownloadVersion,
                        TotalTries = request.TotalTries,
                        Work = request.Work,
                        Ink = request.Ink
                    };
                    studentOtherActivityList.Add(studentOtherActivity);
                }
                List<StudentOtherActivity> otherActivity = activityCount == 0 ? studentOtherActivityList : request.OtherActivity;
                otherActivity.Reverse();

                int statusId = 0;
                int activityStatus = 0;
                var slideResponse = new BaseResponse();
                (statusId, activityStatus, slideResponse) = await SaveStudentSlideActivityAsync(request, otherActivity, sqlTransaction, dbConnection);                
                #endregion

                #region Response 
                if (activityStatus == 1 && !slideResponse.Success)
                {
                    return slideResponse;
                }
                switch (statusId)
                {
                    case 0:
                        response.MessageID = CommonMessage.SuccessID;
                        response.Success = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        response.StatusMessage = ActivityMessage.ExerciseSaved;
                        await sqlTransaction.CommitAsync();
                        break;
                    case 1:
                        response.MessageID = CommonMessage.SuccessID;
                        response.Success = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        response.StatusMessage = ActivityMessage.ExerciseUpdated;
                        await sqlTransaction.CommitAsync();
                        break;
                    case 2:
                        response.MessageID = CommonMessage.SuccessID;
                        response.Success = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        response.StatusMessage = ActivityMessage.ExerciseSubmitted;
                        await sqlTransaction.CommitAsync();
                        break;
                    case 3:
                        response.MessageID = CommonMessage.InvalidParameterID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = ActivityMessage.ExerciseAlreadySubmitted;
                        await sqlTransaction.RollbackAsync();
                        break;
                    case 4:
                        response.MessageID = CommonMessage.InvalidParameterID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = ActivityMessage.OfflineExerciseSaveErrorOnline;
                        await sqlTransaction.RollbackAsync();
                        break;
                    case 5:
                        response.MessageID = CommonMessage.InvalidParameterID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = ActivityMessage.OnlineExerciseSaveErrorOffline;
                        await sqlTransaction.RollbackAsync();
                        break;
                    case 6:
                        response.MessageID = CommonMessage.InvalidParameterID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.StatusMessage = ActivityMessage.OfflineExerciseDownloadVersionError;
                        await sqlTransaction.RollbackAsync();
                        break;
                    default:
                        response.MessageID = CommonMessage.ErrorID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                        await sqlTransaction.RollbackAsync();
                        break;
                }

                return response;               
                #endregion
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

        #region SaveStudentSlideActivityAsync
        public static async Task<(int, int, BaseResponse)> SaveStudentSlideActivityAsync(StudentActivityRequest request, List<StudentOtherActivity> otherActivity, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            TimeToDay time = Time.TimeToDay(request.SpentTime);
            TimeToDay cumulativeTime = Time.TimeToDay(request.CumulativeTimeTaken);
            int SpentDays = time.Days;
            int CumSpentDays = cumulativeTime.Days;
            int statusId = 0;
            BaseResponse response = new();
            foreach (var activity in otherActivity)
            {
                #region Set parameters                
                var activityParameters = new DynamicParameters();
                activityParameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
                activityParameters.Add(ProcedureConstant.IpStudentId, request.StudentID, DbType.Int32);
                activityParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                activityParameters.Add(ProcedureConstant.IpContentId, activity.ContentID, DbType.Int32);
                activityParameters.Add("ip_completed", request.Completed, DbType.Int32);
                activityParameters.Add("ip_total_points", request.TotalPoints, DbType.Double);
                activityParameters.Add("ip_work", activity.Work, DbType.String);
                activityParameters.Add("ip_ink", activity.Ink, DbType.String);
                activityParameters.Add("ip_spent_time", DistributionCommonHelper.StringToTime(time.Hour + ":" + time.Min + ":" + time.Sec), DbType.Time);
                activityParameters.Add("ip_attempts", activity.Attempts, DbType.Int32);
                activityParameters.Add("ip_reset", activity.Reset, DbType.Int32);
                activityParameters.Add("ip_offline", request.Offline, DbType.Int32);
                activityParameters.Add("ip_download_version", request.DownloadVersion, DbType.Int32);
                activityParameters.Add("ip_submit_date", DistributionCommonHelper.StringToDate(request.SubmitDate), DbType.Date);
                activityParameters.Add("ip_spent_days", SpentDays, DbType.Int32);
                activityParameters.Add("ip_total_tries", activity.TotalTries, DbType.Int32);
                activityParameters.Add("ip_cumulative_spent_days", CumSpentDays, DbType.Int32);
                activityParameters.Add("ip_cumulative_time_taken", DistributionCommonHelper.StringToTime(cumulativeTime.Hour + ":" + cumulativeTime.Min + ":" + cumulativeTime.Sec), DbType.Time);
                activityParameters.Add("ip_cumulative_average", request.CumulativeAverage, DbType.Double);
                activityParameters.Add("ip_cumulative_attempts", request.CumulativeAttempts, DbType.Int32);
                activityParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                activityParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                activityParameters.Add("ip_is_parent_id", request.ContentID == activity.ContentID, DbType.Boolean);
                activityParameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                activityParameters.Add("op_activity_type", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveLocalFolder, activityParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                statusId = activityParameters.Get<int>("op_status_id");
                if (statusId > 2)
                {
                    return (statusId, 0, new BaseResponse() { Success = false});
                }
                var activityType = activityParameters.Get<string>("op_activity_type");
                response = await SaveSlideActivityAsync(activity.ContentID, request.StudentID, activityType, activity.Work, activity.Ink, sqlTransaction, dbConnection);
                if (!response.Success)
                {
                    return (statusId, 1, response);
                }                
            }
            return (statusId, 1, response);
        }
        #endregion

        #region SaveSlideActivityAsync
        private static async Task<BaseResponse> SaveSlideActivityAsync(int contentId, int studentId, string activityType, string work, string ink, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            var slideResponse = new BaseResponse();

            #region SmartLabel                
            if (activityType == ServiceConstant.AppSmartLabel)
            {
                slideResponse = await SaveSmartLabelActivity(contentId, studentId, work, sqlTransaction, dbConnection);
                if (slideResponse.StatusCode == StatusCodes.Status400BadRequest)
                {
                    await sqlTransaction.RollbackAsync();

                    return slideResponse;
                }
            }
            #endregion

            #region SmartPaper                
            if (activityType == ServiceConstant.AppSmartPaper)
            {
                slideResponse = await SaveSmartPaperActivity(contentId, studentId, work, ink, sqlTransaction, dbConnection);
                if (slideResponse.StatusCode == StatusCodes.Status400BadRequest)
                {
                    await sqlTransaction.RollbackAsync();

                    return slideResponse;
                }
            }
            #endregion

            #region SmartSlide                
            if (activityType == ServiceConstant.AppSmartSlide)
            {
                slideResponse = await SaveSmartSlideActivity(contentId, studentId, work, sqlTransaction, dbConnection);
                if (slideResponse.StatusCode == StatusCodes.Status400BadRequest)
                {
                    await sqlTransaction.RollbackAsync();

                    return slideResponse;
                }
            }
            #endregion

            return slideResponse;
        }
        #endregion

        #region SaveSmartLabelActivity
        private static async Task<BaseResponse> SaveSmartLabelActivity(int ContentID, int StudentID, string WorkJson, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            try
            {
                if (WorkJson != "")
                {
                    JArray objWork = JArray.Parse(WorkJson);
                    foreach (var workActivity in objWork)
                    {
                        string ModeName = (string)workActivity["ModeName"]!;
                        int Score = (int)workActivity["Score"]!;
                        int Attempts = (int)workActivity["Attempts"]!;
                        string TimeTaken = (workActivity["TimeTaken"] != null && workActivity["TimeTaken"]!.ToString() != "") ? workActivity["TimeTaken"]!.ToString() : LessonpodConstant.DefaultTime;
                        string[] arrSpentTime = TimeTaken.Split(':');
                        _ = int.TryParse(arrSpentTime[0].ToString(), out int SpentMin);
                        _ = int.TryParse(arrSpentTime[1].ToString(), out int SpentSec);
                        TimeSpan timeSpan = TimeSpan.FromSeconds((SpentMin * 60) + SpentSec);
                        TimeTaken = timeSpan.ToString();

                        #region Set Parameters                    
                        var labelParameters = new DynamicParameters();
                        labelParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                        labelParameters.Add(ProcedureConstant.IpStudentId, StudentID, DbType.Int32);
                        labelParameters.Add("ip_mode_name", ModeName, DbType.String);
                        labelParameters.Add("ip_time_taken", DistributionCommonHelper.StringToTime(TimeTaken), DbType.Time);
                        labelParameters.Add("ip_score", Score, DbType.Int32);
                        labelParameters.Add("ip_attempts", Attempts, DbType.Int32);
                        #endregion

                        await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveContentStudentSlReport, labelParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    }
                }                 

                return new BaseResponse()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (NpgsqlException ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
        }
        #endregion

        #region SaveSmartPaperActivity        
        private static async Task<BaseResponse> SaveSmartPaperActivity(int ContentID, int StudentID, string WorkJson, string InkJson, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            try
            {
                var replayParameters = new DynamicParameters();
                replayParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                replayParameters.Add("op_control_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                replayParameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetSpStrokeReplayRectanglesByContent, replayParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outControlRecordSet = replayParameters.Get<string>("op_control_record_set");
                string controlRecordSetQuery = GlobalHelper.StringToString(outControlRecordSet);
                var controlRecordSetData = await dbConnection.QueryAsync<StrokeByContentResponse>(controlRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var outRecordSet = replayParameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<StrokeByContentResponse>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                
                var replayStroke = controlRecordSetData.ToList();
                var replayRect = recordSetData.ToList();

                if (InkJson != "")
                {
                    await SaveSmartPaperInkAsync(contentId: ContentID, studentId: StudentID, inkJson: InkJson, replayStroke: replayStroke, replayRect: replayRect, sqlTransaction: sqlTransaction, dbConnection: dbConnection);
                }                

                if (WorkJson != "")
                {
                    await SaveSmartPaperWorkAsync(contentId: ContentID, studentId: StudentID, workJson: WorkJson, sqlTransaction: sqlTransaction, dbConnection: dbConnection);
                }                

                return new BaseResponse()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (NpgsqlException ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
        }
        #endregion

        #region SaveSmartPaperWorkAsync
        private static async Task SaveSmartPaperWorkAsync(int contentId, int studentId, string workJson, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            #region Work Json  
            JArray objWork = JArray.Parse(workJson);
            foreach (var workActivity in objWork)
            {
                if (workActivity["listControls"] is JArray objControls && objControls.Count > 0)
                {
                    await SaveStudentSPReportAsync(contentId: contentId, studentId: studentId, objControls: objControls, sqlTransaction: sqlTransaction, dbConnection: dbConnection);
                }
            }
            #endregion
        }
        #endregion

        #region SaveStudentSPReportAsync
        private static async Task SaveStudentSPReportAsync(int contentId, int studentId, JArray objControls, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            foreach (var workControl in objControls)
            {
                string answer;
                float score;

                int controlId = DistributionCommonHelper.JTokenToInteger(workControl["CtlId"]!);                
                string ModeName = DistributionCommonHelper.JTokenToString(workControl[LessonpodConstant.CtlValue]!["ModeName"]!);
                if (ModeName == "SmartSlide")
                {
                    JArray? objPopupControls = workControl[LessonpodConstant.CtlValue]?["listControls"] as JArray;
                    score = (objPopupControls![0][LessonpodConstant.CtlScore] != null && objPopupControls[0][LessonpodConstant.CtlScore]?.ToString() != "") ? (float)objPopupControls[0][LessonpodConstant.CtlScore]! : 0;
                    answer = "";
                }
                else
                {
                    score = DistributionCommonHelper.JTokenToFloat(workControl[LessonpodConstant.CtlValue]!["Value"]!);
                    answer = DistributionCommonHelper.JTokenToString(workControl[LessonpodConstant.CtlValue]!["Answer"]!);
                }

                string ElapsedTime = (workControl[LessonpodConstant.ElapsedTime] != null && workControl[LessonpodConstant.ElapsedTime]!.ToString() != "") ? workControl[LessonpodConstant.ElapsedTime]!.ToString() : LessonpodConstant.DefaultTime;
                string[] arrSpentTime = ElapsedTime.Split(':');
                _ = int.TryParse(arrSpentTime[0].ToString(), out int SpentMin);
                _ = int.TryParse(arrSpentTime[1].ToString(), out int SpentSec);
                TimeSpan timeSpan = TimeSpan.FromSeconds((SpentMin * 60) + SpentSec);
                ElapsedTime = timeSpan.ToString();

                var workParameters = new DynamicParameters();
                workParameters.Add(ProcedureConstant.IpContentId, contentId, DbType.Int32);
                workParameters.Add(ProcedureConstant.IpStudentId, studentId, DbType.Int32);
                workParameters.Add("ip_control_id", controlId, DbType.Int32);
                workParameters.Add("ip_answer", answer, DbType.String);
                workParameters.Add("ip_score", score, DbType.Double);
                workParameters.Add("ip_elapsed_time", DistributionCommonHelper.StringToTime(ElapsedTime), DbType.Time);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveContentStudentSpControlReport, workParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        #region SaveSmartPaperInkAsync
        private static async Task SaveSmartPaperInkAsync(int contentId, int studentId, string inkJson, List<StrokeByContentResponse> replayStroke, List<StrokeByContentResponse> replayRect, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            #region Ink json
            JObject jObject = JObject.Parse(inkJson);
            JArray? objInk = jObject["strokeArray"] as JArray;
            foreach (var stroke in objInk!)
            {
                string strokeId = stroke["id"]!.ToString();
                JArray? points = stroke[LessonpodConstant.Points] as JArray;
                float StrokeX = DistributionCommonHelper.JArrayToFloat(points!, 0, LessonpodConstant.X);
                float StrokeY = DistributionCommonHelper.JArrayToFloat(points!, 0, LessonpodConstant.Y);
                int ControlID = DistributionCommonHelper.JTokenToInteger(stroke["id"]!);
                int PenWidth = DistributionCommonHelper.JTokenToInteger(stroke["width"]!);
                bool StrokeAction = DistributionCommonHelper.JTokenToBoolean(stroke["isDeleted"]!);
                string PenColor = DistributionCommonHelper.JTokenToString(stroke["color"]!);
                string StrokePoints = DistributionCommonHelper.JTokenToString(stroke[LessonpodConstant.Points]!);

                #region StartTime
                string StartTime = (stroke["startTime"] != null && stroke["startTime"]!.ToString() != "") ? stroke["startTime"]!.ToString() : LessonpodConstant.DefaultTime;
                string[] arrStartTime = StartTime.Split(':');
                _ = int.TryParse(arrStartTime[0].ToString(), out int StartMin);
                _ = int.TryParse(arrStartTime[1].ToString(), out int StartSec);
                TimeSpan timeSpanStart = TimeSpan.FromSeconds((StartMin * 60) + StartSec);
                StartTime = timeSpanStart.ToString();
                #endregion

                #region ElapsedTime
                string ElapsedTime = (stroke[LessonpodConstant.ElapsedTime] != null && stroke[LessonpodConstant.ElapsedTime]!.ToString() != "") ? stroke[LessonpodConstant.ElapsedTime]!.ToString() : LessonpodConstant.DefaultTime;
                string[] arrSpentTime = ElapsedTime.Split(':');
                _ = int.TryParse(arrSpentTime[0].ToString(), out int SpentMin);
                _ = int.TryParse(arrSpentTime[1].ToString(), out int SpentSec);
                TimeSpan timeSpan = TimeSpan.FromSeconds((SpentMin * 60) + SpentSec);
                ElapsedTime = timeSpan.ToString();
                #endregion

                int inkReplayRectangleId = await InkReplayRectangle(replayStroke: replayStroke, replayRect: replayRect, strokeX: StrokeX, strokeY: StrokeY);

                #region Set parameters
                var strokeParametes = new DynamicParameters();
                strokeParametes.Add(ProcedureConstant.IpContentId, contentId, DbType.Int32);
                strokeParametes.Add(ProcedureConstant.IpStudentId, studentId, DbType.Int32);
                strokeParametes.Add("ip_replay_rectangle_id", inkReplayRectangleId, DbType.Int32);
                strokeParametes.Add("ip_control_id", ControlID, DbType.Int32);
                strokeParametes.Add("ip_pen_color", PenColor, DbType.String);
                strokeParametes.Add("ip_pen_width", PenWidth, DbType.Int32);
                strokeParametes.Add("ip_stroke_action", StrokeAction, DbType.Boolean);
                strokeParametes.Add("ip_elapsed_time", DistributionCommonHelper.StringToTime(ElapsedTime), DbType.Time);
                strokeParametes.Add("ip_start_time", DistributionCommonHelper.StringToTime(StartTime), DbType.Time);
                strokeParametes.Add("ip_stroke_points", StrokePoints, DbType.String);
                strokeParametes.Add("ip_stroke_id", strokeId, DbType.String);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveContentStudentSpStrokeReport, strokeParametes, sqlTransaction, commandType: CommandType.StoredProcedure);
            }
            #endregion
        }
        #endregion

        #region InkReplayRectangle
        private static async Task<int> InkReplayRectangle(List<StrokeByContentResponse> replayStroke, List<StrokeByContentResponse> replayRect, float strokeX, float strokeY)
        {
            bool isReplayRectangle = false;
            int inkReplayRectangleId = 0;

            foreach (var replayRectangle in replayStroke)
            {
                float replayRectX = float.Parse(replayRectangle.X!.ToString().Replace("%", ""));
                float replayRectY = float.Parse(replayRectangle.Y!.ToString().Replace("%", ""));
                if (strokeX >= replayRectX && strokeX <= replayRectX + replayRectangle.Width && strokeY >= replayRectY && strokeY <= replayRectY + replayRectangle.Height)
                {
                    isReplayRectangle = true;
                    inkReplayRectangleId = replayRectangle.ReplayRectangleID;
                    break;
                }
            }

            if (!isReplayRectangle)
            {
                inkReplayRectangleId = replayRect.Count > 0 ? replayRect[0].ReplayRectangleID : 0;
            }

            return await Task.FromResult(inkReplayRectangleId);
        }
        #endregion

        #region SaveSmartSlideActivity        
        private static async Task<BaseResponse> SaveSmartSlideActivity(int ContentID, int StudentID, string WorkJson, NpgsqlTransaction sqlTransaction, IDbConnection dbConnection)
        {
            try
            {
                if (WorkJson != "")
                {
                    JArray objWork = JArray.Parse(WorkJson);
                    foreach (var workActivity in objWork)
                    {
                        JArray? objControls = workActivity["listControls"] as JArray;
                        if (objControls != null && objControls.Count > 0)
                        {
                            foreach (var workControl in objControls)
                            {
                                int CtlId = (int)workControl["CtlId"]!;
                                float Score = (float)workControl[LessonpodConstant.CtlScore]!;

                                #region Set Parameters                            
                                var slideParameters = new DynamicParameters();
                                slideParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                                slideParameters.Add(ProcedureConstant.IpStudentId, StudentID, DbType.Int32);
                                slideParameters.Add("ip_control_id", CtlId, DbType.Int32);
                                slideParameters.Add("ip_score", Score, DbType.Double);
                                slideParameters.Add("ip_control_value", "", DbType.String);
                                #endregion

                                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveContentStudentSsReport, slideParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                }
                
                return new BaseResponse()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (NpgsqlException ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
        }
        #endregion
                
    }
}
