
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Helpers.LessonPod;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Responses.LessonPod.Distribution.SmartPaper;
using BackPack.Library.Responses.LessonPod.Distribution.SmartSlide;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using Npgsql;
using BackPack.Library.Constants;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;
using MassTransit;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class PublishCourseCapsuleRepository : GenericRepository, IPublishCourseCapsuleRepository
    {
        private static int PublishedVersion;

        #region PublishCourseCapsuleAsync        
        public async Task<PublishCourseCapsuleResponse> PublishCourseCapsuleAsync(PublishCourseCapsuleRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add(ProcedureConstant.IpCourseCapsuleId, request.CourseCapsuleID, DbType.Int32);
                parameters.Add("op_version_changed", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.ValidateCourseCapsuleVersion, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int VersionChanged = parameters.Get<int>("op_version_changed");

                if (VersionChanged == 0)
                {
                    return new PublishCourseCapsuleResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CourseCapsuleMessage.PublicCourseNotExist
                    };
                }

                if (VersionChanged == 1)
                {
                    return new PublishCourseCapsuleResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CourseCapsuleMessage.PublicCourseFail
                    };
                }

                #region Set Parameters
                var userParameters = new DynamicParameters();
                userParameters.Add(ProcedureConstant.IpCourseCapsuleId, request.CourseCapsuleID, DbType.Int32);
                userParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                userParameters.Add("op_lessonpod_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetCourseCapsuleById, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = userParameters.Get<string>(ProcedureConstant.OpRecordSet);
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SavePublishCourseCapsule>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outLessonpodRecordSet = userParameters.Get<string>("op_lessonpod_record_set");
                string lessonpodRecordSetQuery = GlobalHelper.StringToString(outLessonpodRecordSet);
                var lessonpodRecordSetData = await dbConnection.QueryAsync<SavePublishCourseCapsuleLessonPod>(lessonpodRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                int publishCourseCapsuleId = await CourseCapsule(recordSetData.ToList(), lessonpodRecordSetData.ToList(), request.PublishType, sqlTransaction, dbConnection);

                if (publishCourseCapsuleId == 0)
                {
                    await sqlTransaction.RollbackAsync();
                    return new PublishCourseCapsuleResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = true,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CourseCapsuleMessage.NotPublished
                    };
                }

                #region Republish Course Capsule               

                #region Set republish parameters 
                var republishParameters = new DynamicParameters();
                republishParameters.Add(ProcedureConstant.IpLoginId, GlobalApplicationProperty.UserID, DbType.Int32);
                republishParameters.Add("ip_publish_course_capsule_id", publishCourseCapsuleId, DbType.Int32);
                republishParameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                republishParameters.Add("ip_publish_type", request.PublishType, DbType.Int32);
                republishParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.RePublishCourseCapsule, DbType.String);
                republishParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                republishParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_republish_course_capsule", republishParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                #endregion

                #region Response
                await sqlTransaction.CommitAsync();

                return new PublishCourseCapsuleResponse()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = CourseCapsuleMessage.PublicCourseCapsule
                };                
                #endregion               
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new PublishCourseCapsuleResponse()
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
                return new PublishCourseCapsuleResponse()
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

        #region CourseCapsule
        private static async Task<int> CourseCapsule(List<SavePublishCourseCapsule> capsule, List<SavePublishCourseCapsuleLessonPod> lessonpod, int publishType, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            int publishedVersion;
            int publishCourseCapsuleId;
            int publishLessonpod = 0;

            for (int index = 0; index < capsule.Count; index++)
            {
                var capsuleItem = capsule[index];
                capsuleItem.PublishType = publishType;
                (publishCourseCapsuleId, publishedVersion) = await SavePublishCourseCapsule(GlobalApplicationProperty.UserID, capsuleItem, sqlTransaction, dbConnection);

                if (publishCourseCapsuleId == 0)
                {
                    return 0;
                }

                PublishedVersion = publishedVersion;

                publishLessonpod = await CourseCapsuleLessonpod(capsuleItem, lessonpod, publishCourseCapsuleId, sqlTransaction, dbConnection);
            }

            return publishLessonpod;
        }
        #endregion

        #region CourseCapsuleLessonpod
        private static async Task<int> CourseCapsuleLessonpod(SavePublishCourseCapsule capsule, List<SavePublishCourseCapsuleLessonPod> lessonpod, int publishCourseCapsuleId, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            for (int index = 0; index < lessonpod.Count; index++)
            {
                var courseCapsuleLessonpod = lessonpod[index];
                int courseCapsuleLessonpodId = courseCapsuleLessonpod.CourseCapsuleLessonPodID;
                string lessonJson = courseCapsuleLessonpod.LessonJson!;
                courseCapsuleLessonpod.ImageURL = GetLessonPodImage(lessonJson!);

                int publishCourseCapsuleLessonpodId = await SavePublishCourseCapsuleLessonPod(GlobalApplicationProperty.UserID, courseCapsuleLessonpod, publishCourseCapsuleId, PublishedVersion, sqlTransaction, dbConnection);

                #region Set Parameters
                var courseParameters = new DynamicParameters();
                courseParameters.Add("ip_course_capsule_lessonpod_id", courseCapsuleLessonpodId, DbType.Int32);
                courseParameters.Add(ProcedureConstant.OpRecordSet, NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + "get_course_capsule_activity_by_pod_id", courseParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outActivityRecordSet = courseParameters.Get<string>(ProcedureConstant.OpRecordSet);
                string activityRecordSetQuery = GlobalHelper.StringToString(outActivityRecordSet);
                var activityRecordSetData = await dbConnection.QueryAsync<CourseCapsuleSlide>(activityRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                CourseCapsuleMethodRequest methodRequest = new()
                {
                    LoginId = GlobalApplicationProperty.UserID,
                    CourseCapsule = capsule,
                    CourseCapsuleLessonPod = courseCapsuleLessonpod,
                    PublishCourseCapsuleId = publishCourseCapsuleId,
                    LessonJson = lessonJson,
                    SqlTransaction = sqlTransaction,
                    DbConnection = dbConnection
                };

                int response = await CourseCapsuleActivity(activityRecordSetData.ToList(), methodRequest, publishCourseCapsuleLessonpodId);
                
                if (response == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
        #endregion

        #region CourseCapsuleActivity
        private static async Task<int> CourseCapsuleActivity(List<CourseCapsuleSlide> activity, CourseCapsuleMethodRequest methodRequest, int publishCourseCapsuleLessonpodId)
        {
            int parentContentId = 0;
            for (int index = 0; index < activity.Count; index++)
            {
                var courseCapsuleActivity = activity[index];
                var slideType = courseCapsuleActivity.SlideType;
                var isContainedView = courseCapsuleActivity.IsContainedView;
                var courseCapsuleActivityId = courseCapsuleActivity.CourseCapsuleActivityID;
                var podActivityDisplayOrder = courseCapsuleActivity.DisplayOrder;
                methodRequest.CourseCapsuleActivity = courseCapsuleActivity;
                if (slideType!.Length == 0)
                {
                    return 0;
                }
                parentContentId = (!isContainedView) ? parentContentId : 0;
                int contentId = await CourseCapsuleActivity(methodRequest, slideType, courseCapsuleActivity.SlideID!);

                if (!isContainedView)
                {
                    parentContentId = contentId;
                }

                if (contentId > 0)
                {
                    CourseCapsulePublishMethodRequest activityRequest = new()
                    {
                        LoginId = GlobalApplicationProperty.UserID,
                        PublishCourseCapsuleLessonpodId = publishCourseCapsuleLessonpodId,
                        ContentId = contentId,
                        CourseCapsuleActivityId = courseCapsuleActivityId,
                        PublishedVersion = PublishedVersion,
                        PodActivityDisplayOrder = podActivityDisplayOrder,
                        SqlTransaction = methodRequest.SqlTransaction,
                        DbConnection = methodRequest.DbConnection
                    };

                    int PublishCourseCapsuleActivityID = await SavePublishCourseCapsuleActivity(activityRequest);

                    if (PublishCourseCapsuleActivityID == 0)
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }

            return 1;
        }
        #endregion

        #region CourseCapsuleActivity
        private static async Task<int> CourseCapsuleActivity(CourseCapsuleMethodRequest request, string slideType, string slideId)
        {
            var contentId = slideType switch
            {
                ServiceConstant.SmartLabel => await SaveSmartLabelAsync(request, slideId),
                ServiceConstant.SmartTile => await SaveSmartTileAsync(request, slideId),
                ServiceConstant.SmartPaper => await SaveSmartPaperAsync(request, slideId),
                ServiceConstant.SmartSlide => await SaveSmartSlideAsync(request, slideId),
                _ => 0,
            };

            return contentId;
        }
        #endregion

        #region SaveSmartLabelAsync
        private static async Task<int> SaveSmartLabelAsync(CourseCapsuleMethodRequest request, string slideId)
        {
            JObject lessonJson = JObject.Parse(request.LessonJson!);
            LessonPodSlideResponse lessonUnitSlideResponse = SmartLabelHelper.SmartLabelJson(lessonJson, slideId!);
            lessonUnitSlideResponse.AppName = ServiceConstant.AppSmartLabel;
            int response = await PutCourseCapsuleToContent(request);

            return response;
        }
        #endregion

        #region SaveSmartTileAsync
        private static async Task<int> SaveSmartTileAsync(CourseCapsuleMethodRequest request, string slideId)
        {
            JObject lessonJson = JObject.Parse(request.LessonJson!);
            LessonPodSlideResponse lessonUnitSlideResponse = SmartTileHelper.SmartTileJson(lessonJson, slideId!);
            lessonUnitSlideResponse.AppName = ServiceConstant.AppSmartTile;
            int response = await PutCourseCapsuleToContent(request);

            return response;
        }
        #endregion

        #region SaveSmartPaperAsync
        private static async Task<int> SaveSmartPaperAsync(CourseCapsuleMethodRequest request, string slideId)
        {
            JObject lessonJson = JObject.Parse(request.LessonJson!);
            LessonPodSlideResponse lessonUnitSlideResponse = SmartPaperHelper.SmartPaperJson(lessonJson, slideId!);
            lessonUnitSlideResponse.AppName = ServiceConstant.AppSmartPaper;

            SmartPaperSlideInputJsonResponse objInputJson = new SmartPaperSlideInputJsonResponse();
            List<SmartPaperInputControlResponse> listSmartPaperInputControl = lessonUnitSlideResponse.SmartPaperInputControls!;
            List<SmartPaperReplayRectangleReportResponse> listSmartPaperReplayRectangleReport = lessonUnitSlideResponse.SmartPaperReplayRectangleReports!;
            List<SmartPaperStrokeResponse> listSmartPaperStrokeReport = lessonUnitSlideResponse.SmartPaperStrokeReports!;
            objInputJson.SPSlideInputC = listSmartPaperInputControl;
            objInputJson.SPSlideInputRR = listSmartPaperReplayRectangleReport;
            objInputJson.SPSlideInputSR = listSmartPaperStrokeReport;

            int response = await PutCourseCapsuleToContent(request);

            if (lessonUnitSlideResponse.SmartPaperInputControls!.Count > 0 || lessonUnitSlideResponse.SmartPaperStrokeReports!.Count > 0)
            {
                await SaveSmartPaperInputControls(response, lessonUnitSlideResponse.SmartPaperInputControls, lessonUnitSlideResponse.SmartPaperReplayRectangleReports!, request.SqlTransaction, request.DbConnection);
            }            

            return response;
        }
        #endregion

        #region SaveSmartSlideAsync
        private static async Task<int> SaveSmartSlideAsync(CourseCapsuleMethodRequest request, string slideId)
        {
            JObject lessonJson = JObject.Parse(request.LessonJson!);
            LessonPodSlideResponse lessonUnitSlideResponse = SmartSlideHelper.SmartSlideJson(lessonJson, slideId!);
            lessonUnitSlideResponse.AppName = ServiceConstant.AppSmartSlide;
            int response = await PutCourseCapsuleToContent(request);

            if (lessonUnitSlideResponse.SmartSlideInputControls!.Count > 0)
            {
                await SaveSmartSlideInputControls(response, lessonUnitSlideResponse.SmartSlideInputControls, request.SqlTransaction, request.DbConnection);
            }

            return response;
        }
        #endregion

        #region SavePublishCourseCapsule
        private static async Task<(int PublishCourseCapsuleID, int PublishedVersion)> SavePublishCourseCapsule(int loginID, SavePublishCourseCapsule courseCapsule, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            try
            {
                #region Set user parameters
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_domain_id", courseCapsule.DomainID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpLoginId, loginID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpSubjectId, courseCapsule.SubjectID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpCourseCapsuleId, courseCapsule.CourseCapsuleID, DbType.Int32);
                userParameters.Add("ip_course_capsule_name", courseCapsule.CourseCapsuleName, DbType.String);
                userParameters.Add("ip_course_capsule_desc", courseCapsule.CourseCapsuleDesc, DbType.String);
                userParameters.Add("ip_image_url", courseCapsule.ImageURL, DbType.String);
                userParameters.Add("ip_publish_type", courseCapsule.PublishType, DbType.Int32);
                userParameters.Add("ip_app_type", courseCapsule.AppType, DbType.String);                    
                userParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.PublishCourseCapsule, DbType.String);
                userParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_publish_course_capsule_id", DbType.Int32, direction: ParameterDirection.Output);
                userParameters.Add("op_published_version", DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_publish_course_capsule", userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int publishCourseCapsuleId = userParameters.Get<int>("op_publish_course_capsule_id");
                int publishedVersion = userParameters.Get<int>("op_published_version");
                #endregion

                #region Response
                if (publishCourseCapsuleId == 0)
                {
                    return (0, 0);
                }
                #endregion
                return (publishCourseCapsuleId, publishedVersion);
            }
            catch (NpgsqlException)
            {
                return (0, 0);
            }
            catch (Exception)
            {
                return (0, 0);
            }
        }
        #endregion

        #region SavePublishCourseCapsuleLessonPod
        private static async Task<int> SavePublishCourseCapsuleLessonPod(int loginID, SavePublishCourseCapsuleLessonPod courseCapsuleLessonpod, int publishCourseCapsuleID, int publishedVersion, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            try
            {
                #region Set user parameters
                var userParameters = new DynamicParameters();
                userParameters.Add(ProcedureConstant.IpLoginId, loginID, DbType.Int32);
                userParameters.Add("ip_publish_course_capsule_id", publishCourseCapsuleID, DbType.Int32);
                userParameters.Add("ip_lesson_unit_id", courseCapsuleLessonpod.LessonUnitID, DbType.Int32);
                userParameters.Add("ip_lesson_id", courseCapsuleLessonpod.LessonID, DbType.Int32);
                userParameters.Add("ip_lesson_name", courseCapsuleLessonpod.LessonName, DbType.String);
                userParameters.Add("ip_lesson_desc", courseCapsuleLessonpod.LessonDesc, DbType.String);
                userParameters.Add("ip_image_url", courseCapsuleLessonpod.ImageURL, DbType.String);
                userParameters.Add("ip_course_capsule_lessonpod_id", courseCapsuleLessonpod.CourseCapsuleLessonPodID, DbType.Int32);
                userParameters.Add("ip_published_version", publishedVersion, DbType.Int32);
                userParameters.Add("ip_course_capsule_folder_id", courseCapsuleLessonpod.CourseCapsuleFolderID, DbType.Int32);
                userParameters.Add("ip_display_order", courseCapsuleLessonpod.DisplayOrder, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.PublishCourseCapsule, DbType.String);
                userParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_publish_course_capsule_lessonpod_id", DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_publish_course_capsule_lessonpod", userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int PublishCourseCapsuleLessonPodID = userParameters.Get<int>("op_publish_course_capsule_lessonpod_id");
                #endregion

                return PublishCourseCapsuleLessonPodID;

            }
            catch (NpgsqlException)
            {
                return 0;

            }
            catch (Exception)
            {
                return 0;

            }
        }
        #endregion

        #region GetLessonPodImage
        private static string GetLessonPodImage(string lessonJson)
        {
            string PodImage = "";
            try
            {
                JObject jobject = JObject.Parse(lessonJson);
                JObject objGlobalConfig = (JObject)jobject["present"]!["globalConfig"]!;
                PodImage = (objGlobalConfig["thumbnail"] != null && objGlobalConfig["thumbnail"]!.ToString() != "") ? objGlobalConfig["thumbnail"]!.ToString() : "";
                return PodImage;
            }
            catch (Exception)
            {
                return PodImage;
            }
        }
        #endregion

        #region PutCourseCapsuleToContent
        private static async Task<int> PutCourseCapsuleToContent(CourseCapsuleMethodRequest request)
        {
            try
            {
                #region Set user parameters
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_publish_course_capsule_id", request.PublishCourseCapsuleId, DbType.Int32);
                userParameters.Add("ip_course_capsule_activity_id", request.CourseCapsuleActivity.CourseCapsuleActivityID, DbType.Int32);
                userParameters.Add("ip_domain_id", request.CourseCapsule.DomainID, DbType.Int32);
                userParameters.Add("ip_content_name", request.CourseCapsuleActivity.SlideName, DbType.String);
                userParameters.Add("ip_subject_id", request.CourseCapsule.SubjectID, DbType.Int32);
                userParameters.Add("ip_app_name", request.LessonPodSlideResponse.AppName, DbType.String);
                userParameters.Add("ip_activity_type", request.CourseCapsuleActivity.ActivityType, DbType.String);
                userParameters.Add("ip_content_mode", request.CourseCapsuleActivity.ContentMode, DbType.String);
                userParameters.Add("ip_content_xml", request.LessonPodSlideResponse.SlideJson, DbType.String);
                userParameters.Add(ProcedureConstant.IpLoginId, request.LoginId, DbType.Int32);
                userParameters.Add("ip_lesson_id", request.CourseCapsuleLessonPod.LessonID, DbType.Int32);
                userParameters.Add("ip_min_score", request.CourseCapsuleActivity.MinScore, DbType.Int32);
                userParameters.Add("ip_min_time_min", request.CourseCapsuleActivity.MinTimeMin, DbType.Int32);
                userParameters.Add("ip_min_time_sec", request.CourseCapsuleActivity.MinTimeSec, DbType.Int32);
                userParameters.Add("ip_max_time_min", request.CourseCapsuleActivity.MaxTimeMin, DbType.Int32);
                userParameters.Add("ip_max_time_sec", request.CourseCapsuleActivity.MaxTimeSec, DbType.Int32);
                userParameters.Add("ip_follow_the_flow", request.CourseCapsuleActivity.FollowTheFlow, DbType.Boolean);
                userParameters.Add("ip_auto_hint", request.CourseCapsuleActivity.AutoHint, DbType.Boolean);
                userParameters.Add("ip_image_url", request.LessonPodSlideResponse.ImageURL, DbType.String);
                userParameters.Add("ip_search_tag", request.LessonPodSlideResponse.SearchTag, DbType.String);
                userParameters.Add("ip_search_name", request.CourseCapsuleActivity.SlideName, DbType.String);
                userParameters.Add("ip_file_size", request.LessonPodSlideResponse.FileSize, DbType.Int32);
                userParameters.Add("ip_parent_content_id", request.ParentContentId, DbType.Int32);
                userParameters.Add("ip_slide_id", request.CourseCapsuleActivity.SlideID, DbType.String);
                userParameters.Add("ip_is_contained_view", request.CourseCapsuleActivity.IsContainedView, DbType.Boolean);
                userParameters.Add("ip_total_points", request.LessonPodSlideResponse.TotalPoints, DbType.Double);
                userParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.PublishCourseCapsule, DbType.String);
                userParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_content_id", DbType.Int32, direction: ParameterDirection.Output);

                await request.DbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "publish_course_capsule_content", userParameters, request.SqlTransaction, commandType: CommandType.StoredProcedure);
                int ContentID = userParameters.Get<int>("op_content_id");
                #endregion

                return ContentID;
            }
            catch (NpgsqlException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region SaveSmartPaperInputControls
        private static async Task<int> SaveSmartPaperInputControls(int ContentID, List<SmartPaperInputControlResponse> listSmartPaperInputControl, List<SmartPaperReplayRectangleReportResponse> listSmartPaperReplayRectangleReport, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            bool IsExist = false;
            int ReplayControlID = 0;
            try
            {
                #region listSmartPaperReplayRectangleReport
                foreach (var replayControl in listSmartPaperReplayRectangleReport)
                {
                    #region Set replay rectangle parameters
                    var replayParameters = new DynamicParameters();
                    replayParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                    replayParameters.Add("ip_control_id", replayControl.ControlID, DbType.Int32);
                    replayParameters.Add("ip_plugin_name", replayControl.PluginName, DbType.String);
                    replayParameters.Add("ip_control_tag", replayControl.ControlTag, DbType.String);
                    replayParameters.Add("ip_control_name", replayControl.ControlName, DbType.String);
                    replayParameters.Add("ip_height", replayControl.Height, DbType.String);
                    replayParameters.Add("ip_width", replayControl.Width, DbType.String);
                    replayParameters.Add("ip_x", replayControl.X, DbType.String);
                    replayParameters.Add("ip_y", replayControl.Y, DbType.String);
                    replayParameters.Add("ip_x_left", replayControl.XLeft, DbType.Double);
                    replayParameters.Add("ip_x_right", replayControl.XRight, DbType.Double);
                    replayParameters.Add("ip_y_top", replayControl.YTop, DbType.Double);
                    replayParameters.Add("ip_y_bottom", replayControl.YBottom, DbType.Double);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_content_sp_replay_rectangles", replayParameters, sqlTransaction, commandType: CommandType.StoredProcedure);                    
                }
                #endregion

                #region listSmartPaperReplayRectangleReport
                foreach (var control in listSmartPaperInputControl)
                {
                    foreach (var replayRectangle in listSmartPaperReplayRectangleReport)
                    {
                        if (control.Left >= replayRectangle.XLeft && control.Left <= replayRectangle.XRight && control.Top >= replayRectangle.YTop && control.Top <= replayRectangle.YBottom)
                        {
                            ReplayControlID = replayRectangle.ControlID;
                            IsExist = true;
                            break;
                        }
                    }
                    if (!IsExist)
                    {
                        ReplayControlID = 0;
                    }

                    #region Set controls parameters
                    var controlParameters = new DynamicParameters();
                    controlParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                    controlParameters.Add("ip_control_id", control.ControlID, DbType.Int32);
                    controlParameters.Add("ip_plugin_name", control.PluginName, DbType.String);
                    controlParameters.Add("ip_control_tag", control.ControlTag, DbType.String);
                    controlParameters.Add("ip_control_name", control.ControlName, DbType.String);
                    controlParameters.Add("ip_total_points", control.TotalPoints, DbType.Double);
                    controlParameters.Add("ip_replay_control_id", ReplayControlID, DbType.Int32);
                    controlParameters.Add("op_content_sp_control_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_content_sp_controls", controlParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    int ContentSPControlID = controlParameters.Get<int>("op_content_sp_control_id");

                    if (ContentSPControlID == 0)
                    {
                        return 0;
                    }
                }
                #endregion
                return 1;
            }
            catch (NpgsqlException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion  

        #region SaveSmartSlideInputControls 
        private static async Task<int> SaveSmartSlideInputControls(int ContentID, List<SmartSlideInputControlResponse> listSmartSlideInputControl, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            try
            {
                #region listSmartPaperReplayRectangleReport 
                foreach (var control in listSmartSlideInputControl)
                {
                    #region Set control parametes
                    var controlParameters = new DynamicParameters();
                    controlParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                    controlParameters.Add("ip_control_id", control.ControlID, DbType.Int32);
                    controlParameters.Add("ip_plugin_name", control.PluginName, DbType.String);
                    controlParameters.Add("ip_control_tag", control.ControlTag, DbType.String);
                    controlParameters.Add("ip_control_name", control.ControlName, DbType.String);
                    controlParameters.Add("ip_total_points", control.TotalPoints, DbType.Double);
                    controlParameters.Add("ip_question", control.Question, DbType.String);
                    controlParameters.Add("op_content_smart_slide_control_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_content_smart_slide_control", controlParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    int ContentSmartSlideControlID = controlParameters.Get<int>("op_content_smart_slide_control_id");

                    if (ContentSmartSlideControlID > 0)
                    {
                        foreach (var controlDetails in control.InputControlDetails!)
                        {
                            #region Set detail parameter
                            var detailParameters = new DynamicParameters();
                            detailParameters.Add("ip_content_smart_slide_control_id", ContentSmartSlideControlID, DbType.Int32);
                            detailParameters.Add("ip_control_type", controlDetails.ControlType, DbType.String);
                            detailParameters.Add("ip_option_data", controlDetails.OptionData, DbType.String);
                            detailParameters.Add("ip_option_value", controlDetails.OptionValue, DbType.Int32);
                            detailParameters.Add("ip_option_index", controlDetails.OptionIndex, DbType.Int32);
                            detailParameters.Add("ip_correct_value", controlDetails.CorrectValue, DbType.String);
                            #endregion

                            await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_content_smart_slide_control_details", detailParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                        }
                    }

                }
                #endregion 
                return 1;
            }
            catch (NpgsqlException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
        
        #region SavePublishCourseCapsuleActivity
        private static async Task<int> SavePublishCourseCapsuleActivity(CourseCapsulePublishMethodRequest request)
        {
            try
            {
                #region Set user parameters 
                var userParameters = new DynamicParameters();
                userParameters.Add(ProcedureConstant.IpLoginId, request.LoginId, DbType.Int32);
                userParameters.Add("ip_publish_course_capsule_lessonpod_id", request.PublishCourseCapsuleLessonpodId, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpContentId, request.ContentId, DbType.Int32);
                userParameters.Add("ip_course_capsule_activity_id", request.CourseCapsuleActivityId, DbType.Int32);
                userParameters.Add("ip_published_version", PublishedVersion, DbType.Int32);
                userParameters.Add("ip_slide_input_json", request.SlideInputJson, DbType.String);
                userParameters.Add("ip_display_order", request.PodActivityDisplayOrder, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpActivityDesc, LogMessage.PublishCourseCapsule, DbType.String);
                userParameters.Add(ProcedureConstant.IpActivityBy, GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add(ProcedureConstant.IpUserTypeId, GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_publish_course_capsule_activity_id", DbType.Int32, direction: ParameterDirection.Output);

                await request.DbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "save_publish_course_capsule_activity", userParameters, request.SqlTransaction, commandType: CommandType.StoredProcedure);
                int PublishCourseCapsuleActivityID = userParameters.Get<int>("op_publish_course_capsule_activity_id");
                #endregion 

                return PublishCourseCapsuleActivityID;
            }
            catch (NpgsqlException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}
