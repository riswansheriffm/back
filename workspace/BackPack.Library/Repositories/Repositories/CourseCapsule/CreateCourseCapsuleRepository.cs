using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class CreateCourseCapsuleRepository : GenericRepository, ICreateCourseCapsuleRepository
    {
        #region CreateCourseCapsuleAsync        
        public async Task<BaseResponse> CreateCourseCapsuleAsync(CreateCourseCapsuleRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {

                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                parameters.Add("ip_course_capsule_name", request.CourseCapsuleName, DbType.String);
                parameters.Add("ip_course_capsule_desc", request.CourseCapsuleDesc, DbType.String);
                parameters.Add("ip_image_url", request.ImageURL, DbType.String);
                parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                parameters.Add("ip_app_type", request.AppType, DbType.String);
                parameters.Add("ip_activity_desc", (request.CourseCapsuleID > 0) ? LogMessage.UpdateCourseCapsule : LogMessage.CreateCourseCapsule, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_course_capsule_id", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveCourseCapsule, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int CourseCapsuleID = parameters.Get<int>("op_course_capsule_id");

                if (CourseCapsuleID > 0)
                {

                    for (int index = 0; index < request.LessonPods?.Count; index++)
                    {
                        CreateCourseCapsuleLessonPod capsuleLessonpod = request.LessonPods[index];

                        #region Set Lesson parameters                    
                        var lessonParameters = new DynamicParameters();
                        lessonParameters.Add("ip_course_capsule_id", CourseCapsuleID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_unit_id", capsuleLessonpod.LessonUnitID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_id", capsuleLessonpod.LessonID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_name", capsuleLessonpod.LessonName, DbType.String);
                        lessonParameters.Add("ip_lesson_desc", capsuleLessonpod.LessonDesc, DbType.String);
                        lessonParameters.Add("ip_author_id", GlobalApplicationProperty.UserID, DbType.Int32);
                        lessonParameters.Add("ip_access_type", capsuleLessonpod.AccessType, DbType.String);
                        lessonParameters.Add("ip_lesson_pod_version", capsuleLessonpod.LessonPodVersion, DbType.Int32);
                        lessonParameters.Add("ip_course_capsule_folder_id", capsuleLessonpod.CourseCapsuleFolderID, DbType.Int32);
                        lessonParameters.Add("ip_activity_desc", LogMessage.CreateCourseCapsuleLessonpod, DbType.String);
                        lessonParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        lessonParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        lessonParameters.Add("op_course_capsule_lessonpod_id", DbType.Int64, direction: ParameterDirection.Output);
                        #endregion

                        await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveCourseCapsuleLessonpod, lessonParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                        int courseCapsuleLessonpodId = lessonParameters.Get<int>("op_course_capsule_lessonpod_id");

                        if (courseCapsuleLessonpodId > 0)
                        {
                            await SaveCapsuleActivityAsync(courseCapsuleLessonpodId: courseCapsuleLessonpodId, slideActivity: capsuleLessonpod.Activities!, sqlTransaction: sqlTransaction, dbConnection: dbConnection);
                        }
                    }

                    await sqlTransaction.CommitAsync();
                    return new BaseResponse
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = CourseCapsuleMessage.CreateCourseCapsule
                    };
                }

                await sqlTransaction.RollbackAsync();
                return new BaseResponse
                {
                    MessageID = CommonMessage.FailID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = CommonMessage.BadRequestMessage
                };
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new BaseResponse
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeSqlException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
                return new BaseResponse
                {
                    MessageID = CommonMessage.ExceptionID,
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = ex.Message,
                    ExceptionType = CommonMessage.ExceptionTypeException,
                    ExceptionMessage = ex.StackTrace
                };
            }
            finally
            {
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region SaveCapsuleActivityAsync
        private static async Task SaveCapsuleActivityAsync(int courseCapsuleLessonpodId, List<CreateCourseCapsuleActivities> slideActivity, NpgsqlTransaction sqlTransaction, NpgsqlConnection dbConnection)
        {
            for (int indexs = 0; indexs < slideActivity.Count; indexs++)
            {
                CreateCourseCapsuleActivities activity = slideActivity[indexs];

                #region Set Lesson parameters                    
                var activityParameters = new DynamicParameters();
                activityParameters.Add("ip_course_capsule_lessonpod_id", courseCapsuleLessonpodId, DbType.Int32);
                activityParameters.Add("ip_slide_id", activity.SlideID, DbType.String);
                activityParameters.Add("ip_slide_type", activity.SlideType, DbType.String);
                activityParameters.Add("ip_slide_name", activity.SlideName, DbType.String);
                activityParameters.Add("ip_is_selected", activity.IsSelected != 0, DbType.Boolean);
                activityParameters.Add("ip_activity_type", activity.ActivityType, DbType.String);
                activityParameters.Add("ip_min_score", activity.MinScore, DbType.Int32);
                activityParameters.Add("ip_min_time_min", activity.MinTimeMin, DbType.Int32);
                activityParameters.Add("ip_min_time_sec", activity.MinTimeSec, DbType.Int32);
                activityParameters.Add("ip_max_time_min", activity.MaxTimeMin, DbType.Int32);
                activityParameters.Add("ip_max_time_sec", activity.MaxTimeSec, DbType.Int32);
                activityParameters.Add("ip_follow_the_flow", activity.FollowTheFlow != 0, DbType.Boolean);
                activityParameters.Add("ip_auto_hint", activity.AutoHint != 0, DbType.Boolean);
                activityParameters.Add("ip_content_mode", activity.ContentMode, DbType.String);
                activityParameters.Add("ip_is_contained_view", activity.IsContainedView, DbType.Boolean);
                activityParameters.Add("ip_activity_desc", LogMessage.CreateCourseCapsuleLessonpod, DbType.String);
                activityParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                activityParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveCourseCapsuleActivity, activityParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}
