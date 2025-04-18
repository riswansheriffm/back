using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod.Distribution.SmartPaper;
using BackPack.Library.Responses.LessonPod.Distribution.SmartSlide;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Dependency.Library.Responses;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;
using BackPack.Library.Helpers.LessonPod;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public static class LessonPodDistributionCommon
    {
        #region SaveContentAsync
        public static async Task<int> SaveContentAsync(SaveContentResponse saveContentResponse, LessonPodSlideRequest lessonUnitSlideRequest, LessonPodSlideResponse lessonUnitSlideResponse, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters            
            var parameters = new DynamicParameters();
            parameters.Add("ip_lesson_unit_dist_id", saveContentResponse.LessonUnitDistID, DbType.Int32);
            parameters.Add("ip_parent_content_id", saveContentResponse.ParentContentID, DbType.Int32);
            parameters.Add("ip_slide_id", saveContentResponse.SlideID, DbType.String);
            parameters.Add("ip_is_contained_view", saveContentResponse.IsContainedView, DbType.Boolean);
            parameters.Add("ip_is_canvas", lessonUnitSlideResponse.IsCanvas, DbType.Boolean);
            parameters.Add("ip_domain_id", saveContentResponse.DomainID, DbType.Int32);
            parameters.Add("ip_content_name", saveContentResponse.ContentName, DbType.String);
            parameters.Add("ip_course_id", saveContentResponse.CourseID, DbType.Int32);
            parameters.Add("ip_app_name", lessonUnitSlideResponse.AppName, DbType.String);
            parameters.Add("ip_activity_type", lessonUnitSlideRequest.ActivityType, DbType.String);
            parameters.Add("ip_content_mode", lessonUnitSlideRequest.ContentMode, DbType.String);
            parameters.Add("ip_content_xml", lessonUnitSlideResponse.SlideJson, DbType.String);
            parameters.Add("ip_login_id", saveContentResponse.LoginID, DbType.Int32);
            parameters.Add("ip_lesson_id", saveContentResponse.LessonID, DbType.Int32);
            parameters.Add("ip_attempts", lessonUnitSlideRequest.Attempts, DbType.Int32);
            parameters.Add("ip_min_time", lessonUnitSlideRequest.MinTime, DbType.Int32);
            parameters.Add("ip_max_time", lessonUnitSlideRequest.MaxTime, DbType.Int32);
            parameters.Add("ip_mastery_percentage", lessonUnitSlideRequest.MasteryPercentage, DbType.Int32);
            parameters.Add("ip_optional", lessonUnitSlideRequest.Optional, DbType.Boolean);
            parameters.Add("ip_no_of_items", lessonUnitSlideRequest.NoOfItems, DbType.Int32);
            parameters.Add("ip_target_date_of_completion", DistributionCommonHelper.StringToDate(lessonUnitSlideRequest.TargetDateOfCompletion), DbType.Date);
            parameters.Add("ip_target_time_of_completion", DistributionCommonHelper.StringToTime(lessonUnitSlideRequest.TargetTimeOfCompletion), DbType.Time);
            parameters.Add("ip_total_points", lessonUnitSlideResponse.TotalPoints, DbType.Int32);
            parameters.Add("ip_is_readonly", lessonUnitSlideResponse.IsReadonly, DbType.Boolean);
            parameters.Add("ip_search_tag", lessonUnitSlideResponse.SearchTag, DbType.String);
            parameters.Add("ip_search_name", saveContentResponse.ContentName, DbType.String);
            parameters.Add("ip_flag_visible_to_parent", saveContentResponse.FlagVisibleToParent, DbType.Boolean);
            parameters.Add("ip_file_size", lessonUnitSlideResponse.FileSize, DbType.Int32);
            parameters.Add("ip_follow_the_flow", (lessonUnitSlideRequest.FollowTheFlow == 1), DbType.Boolean);
            parameters.Add("ip_auto_hint", (lessonUnitSlideRequest.AutoHint == 1), DbType.Boolean);
            parameters.Add("ip_published_content_id", saveContentResponse.PublishedContentID, DbType.Int32);
            parameters.Add("ip_lesson_pod_type", saveContentResponse.LessonPodType, DbType.String);
            parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
            parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
            parameters.Add("op_content_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.DistributeLessonUnitContent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

            int ContentID = parameters.Get<int>("op_content_id");

            return ContentID;
        }
        #endregion

        #region SaveStudentsWithContentAsync
        public static async Task SaveStudentsWithContentAsync(int ContentID, string StudentIDs, int FlagVisibleToParent, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters
            var parameters = new DynamicParameters();
            parameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
            parameters.Add("ip_student_ids", StudentIDs, DbType.String);
            parameters.Add("ip_flag_visible_to_parent", (FlagVisibleToParent == 1), DbType.Boolean);
            parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.MapStudentsWithContent, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region SaveStudentsWithDistributedLessonAsync
        public static async Task<int> SaveStudentsWithDistributedLessonAsync(int LessonUnitDistID, string StudentIDs, LessonPodDistributionRequest request, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters
            var parameters = new DynamicParameters();
            parameters.Add("ip_lesson_unit_dist_id", LessonUnitDistID, DbType.Int32);
            parameters.Add("ip_student_ids", StudentIDs, DbType.String);
            parameters.Add("ip_target_date_of_completion", DistributionCommonHelper.StringToDate(request.TargetDateOfCompletion), DbType.Date);
            parameters.Add("ip_target_time_of_completion", DistributionCommonHelper.StringToTime(request.TargetTimeOfCompletion), DbType.Time);
            parameters.Add("ip_flag_visible_to_parent", (request.FlagVisibleToParent == 1), DbType.Boolean);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.MapStudentsWithDistributedLesson, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

            return 1;
        }
        #endregion

        #region SaveSmartPaperControlAsync
        public static async Task<int> SaveSmartPaperControlAsync(int ContentID, List<SmartPaperInputControlResponse> listSmartPaperInputControl, List<SmartPaperReplayRectangleReportResponse> listSmartPaperReplayRectangleReport, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            int Response = 0;

            #region Replay rectangle
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

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveContentSpReplayRectangles, replayParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
            }
            #endregion

            #region Slide controls
            foreach (var control in listSmartPaperInputControl)
            {
                #region Find replay Rectangle
                int ReplayControlID = 0;
                foreach (var replayRectangle in listSmartPaperReplayRectangleReport)
                {
                    if (control.Left >= replayRectangle.XLeft && control.Left <= replayRectangle.XRight && control.Top >= replayRectangle.YTop && control.Top <= replayRectangle.YBottom)
                    {
                        ReplayControlID = replayRectangle.ControlID;
                        break;
                    }
                }
                #endregion

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

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveContentSpControls, controlParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int ContentSPControlID = controlParameters.Get<int>("op_content_sp_control_id");

                if (ContentSPControlID == 0)
                {
                    Response = 1;
                    break;
                }
            }
            #endregion

            return Response;
        }
        #endregion

        #region SaveSmartSlideControlAsync
        public static async Task<int> SaveSmartSlideControlAsync(int ContentID, List<SmartSlideInputControlResponse> listSmartSlideInputControl, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            int Response = 0;
            foreach (var control in listSmartSlideInputControl)
            {
                #region Set data                
                int ControlID = control.ControlID;
                string? PluginName = control.PluginName;
                string? ControlTag = control.ControlTag;
                string? ControlName = control.ControlName;
                float TotalPoints = control.TotalPoints;
                string Question = (control.Question != null) ? control.Question : "";
                #endregion

                #region Set control parametes
                var controlParameters = new DynamicParameters();
                controlParameters.Add(ProcedureConstant.IpContentId, ContentID, DbType.Int32);
                controlParameters.Add("ip_control_id", ControlID, DbType.Int32);
                controlParameters.Add("ip_plugin_name", PluginName, DbType.String);
                controlParameters.Add("ip_control_tag", ControlTag, DbType.String);
                controlParameters.Add("ip_control_name", ControlName, DbType.String);
                controlParameters.Add("ip_total_points", TotalPoints, DbType.Double);
                controlParameters.Add("ip_question", Question, DbType.String);
                controlParameters.Add("op_content_smart_slide_control_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveContentSmartSlideControl, controlParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int ContentSmartSlideControlID = controlParameters.Get<int>("op_content_smart_slide_control_id");

                #region Save details
                if (ContentSmartSlideControlID > 0)
                {
                    foreach (var controlDetails in control.InputControlDetails!)
                    {
                        #region Set detail data
                        string? ControlType = controlDetails.ControlType;
                        string OptionData = "";
                        int OptionValue = controlDetails.OptionValue;
                        string? CorrectValue = controlDetails.CorrectValue;
                        int OptionIndex = controlDetails.OptionIndex;
                        #endregion

                        #region Set detail parameter
                        var detailParameters = new DynamicParameters();
                        detailParameters.Add("ip_content_smart_slide_control_id", ContentSmartSlideControlID, DbType.Int32);
                        detailParameters.Add("ip_control_type", ControlType, DbType.String);
                        detailParameters.Add("ip_option_data", OptionData, DbType.String);
                        detailParameters.Add("ip_option_value", OptionValue, DbType.Int32);
                        detailParameters.Add("ip_option_index", OptionIndex, DbType.Int32);
                        detailParameters.Add("ip_correct_value", CorrectValue, DbType.String);
                        #endregion

                        await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveContentSmartSlideControlDetails, detailParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    }
                }
                else
                {
                    Response = 1;
                    break;
                }
                #endregion
            }
            return Response;
        }
        #endregion

        #region SaveLessonPodDistributionAuditLogAsync
        public static async Task SaveLessonPodDistributionAuditLogAsync(DistributionAuditLogResponse request, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters
            var parameters = new DynamicParameters();
            parameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
            parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
            parameters.Add("ip_distribution_type_id", request.DistributionTypeID, DbType.Int32);
            parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
            parameters.Add("ip_activity_description", request.ActivityDescription, DbType.String);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.CreateAuditLogForLessonUnitDistribution, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region DistributionStudentList
        public static async Task<string> DistributionStudentList(string whomToDistribute, int courseId, string groupIds, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            string response = "";

            try
            {
                #region Set parameters                
                var studentParameters = new DynamicParameters();
                studentParameters.Add("ip_whom_to_distribute", whomToDistribute, DbType.String);
                studentParameters.Add("ip_course_id", courseId, DbType.Int32);
                studentParameters.Add("ip_group_ids", groupIds, DbType.String);
                studentParameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaStudents + ProcedureConstant.GetStudentsForDistribution, studentParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = studentParameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<int>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                response = string.Join(",", recordSetData.ToList());
            }
            catch (Exception)
            {
                return response;
            }

            return response;
        }
        #endregion
    }
}
