using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod.Distribution;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public static class LessonPodCommon
    {
        #region SaveLessonPodActivityAsync
        public static async Task<int> SaveLessonPodActivityAsync(int LessonUnitID, LessonPodSlideListResponse slideActivity, LessonPodSlideResponse lessonPodSlideResponse, IDbConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters
            var parameters = new DynamicParameters();
            parameters.Add("ip_lesson_unit_id", LessonUnitID, DbType.Int32);
            parameters.Add("ip_is_contained_view", slideActivity.IsContentView, DbType.Boolean);
            parameters.Add("ip_parent_slide_id", slideActivity.ParentSlideID, DbType.String);
            parameters.Add("ip_slide_id", slideActivity.SlideID, DbType.String);
            parameters.Add("ip_slide_type", slideActivity.SlideType, DbType.String);
            parameters.Add("ip_slide_name", slideActivity.SlideName, DbType.String);
            parameters.Add("ip_app_name", lessonPodSlideResponse.AppName, DbType.String);
            parameters.Add("ip_activity_json", lessonPodSlideResponse.SlideJson, DbType.String);
            parameters.Add("ip_display_order", slideActivity.DisplayOrder, DbType.Int32);
            parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
            parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
            parameters.Add("ip_activity_desc", LogMessage.CreateLessonpodActivity, DbType.String);
            parameters.Add("ip_is_canvas", slideActivity.IsCanvas, DbType.Boolean);
            parameters.Add("ip_is_child_page", slideActivity.IsChildPage, DbType.Boolean);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveLessonUnitActivity, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

            return 1;
        }
        #endregion

        #region SaveLessonPodPreviewActivityAsync
        public static async Task<int> SaveLessonPodPreviewActivityAsync(CreatePreviewLessonPodActivityRequest request, LessonPodSlideListResponse slideActivity, LessonPodSlideResponse lessonPodSlideResponse, IDbConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            #region Set parameters
            var parameters = new DynamicParameters();
            parameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
            parameters.Add("ip_is_contained_view", slideActivity.IsContentView, DbType.Boolean);
            parameters.Add("ip_slide_id", slideActivity.SlideID, DbType.String);
            parameters.Add("ip_slide_type", slideActivity.SlideType, DbType.String);
            parameters.Add("ip_slide_name", slideActivity.SlideName, DbType.String);
            parameters.Add("ip_app_name", lessonPodSlideResponse.AppName, DbType.String);
            parameters.Add("ip_activity_json", lessonPodSlideResponse.SlideJson, DbType.String);
            parameters.Add("ip_lesson_name", request.LessonName, DbType.String);
            parameters.Add("ip_lesson_desc", request.LessonDesc, DbType.String);
            parameters.Add("ip_parent_slide_id", slideActivity.ParentSlideID, DbType.String);
            parameters.Add("ip_is_canvas", slideActivity.IsCanvas, DbType.Boolean);
            parameters.Add("ip_display_order", slideActivity.DisplayOrder, DbType.Int32);
            parameters.Add("ip_is_child_page", slideActivity.IsChildPage, DbType.Boolean);
            #endregion

            await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveLessonUnitPreviewActivity, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

            return 1;
        }
        #endregion

    }
}
