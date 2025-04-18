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
    public class EditCourseCapsuleRepository : GenericRepository, IEditCourseCapsuleRepository
    {
        #region EditCourseCapsuleAsync        
        public async Task<BaseResponse> EditCourseCapsuleAsync(EditCourseCapsuleRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("ip_login_id", request.LoginID, DbType.Int32);
                parameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                parameters.Add("ip_course_capsule_name", request.CourseCapsuleName, DbType.String);
                parameters.Add("ip_course_capsule_desc", request.CourseCapsuleDesc, DbType.String);
                parameters.Add("ip_image_url", request.ImageURL, DbType.String);
                parameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsule, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_course_capsule_id", DbType.Int64, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryMultipleAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.EditCourseCapsule, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int CourseCapsuleID = parameters.Get<int>("op_course_capsule_id");

                if (CourseCapsuleID > 0)
                {
                    for (int index = 0; index < request.LessonPods?.Count; index++)
                    {
                        EditCourseCapsuleLessonPod lessonpod = request.LessonPods[index];

                        #region Set Lesson parameters                    
                        var lessonParameters = new DynamicParameters();
                        lessonParameters.Add("ip_course_capsule_lessonpod_id", lessonpod.CourseCapsuleLessonPodID, DbType.Int32);
                        lessonParameters.Add("ip_course_capsule_id", CourseCapsuleID, DbType.Int32);
                        lessonParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_pod_deleted", lessonpod.LessonPodDeleted, DbType.Int32);
                        lessonParameters.Add("ip_is_deleted", lessonpod.IsDeleted, DbType.Int32);
                        lessonParameters.Add("ip_lesson_unit_id", lessonpod.LessonUnitID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_id", lessonpod.LessonID, DbType.Int32);
                        lessonParameters.Add("ip_lesson_name", lessonpod.LessonName, DbType.String);
                        lessonParameters.Add("ip_lesson_desc", lessonpod.LessonDesc, DbType.String);
                        lessonParameters.Add("ip_author_id", lessonpod.AuthorID, DbType.Int32);
                        lessonParameters.Add("ip_access_type", lessonpod.AccessType, DbType.String);
                        lessonParameters.Add("ip_lesson_pod_version", lessonpod.LessonPodVersion, DbType.Int32);
                        lessonParameters.Add("ip_course_capsule_folder_id", lessonpod.CourseCapsuleFolderID, DbType.Int32);
                        lessonParameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsule, DbType.String);
                        lessonParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        lessonParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        lessonParameters.Add("op_course_capsule_lessonpod_id", DbType.Int64, direction: ParameterDirection.Output);
                        #endregion

                        var userResult = await Task.FromResult(dbConnection!.Query(ServiceConstant.SchemaLessonpods + ProcedureConstant.EditCourseCapsuleLessonpod, lessonParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                        int CourseCapsuleLessonPodID = lessonParameters.Get<int>("op_course_capsule_lessonpod_id");
                        int LessonPodDeleted = lessonpod.LessonPodDeleted;
                        int IsDeleted = lessonpod.IsDeleted;

                        if (CourseCapsuleLessonPodID > 0 && LessonPodDeleted == 0 && IsDeleted == 0)
                        {
                            for (int indexs = 0; indexs < lessonpod.Activities?.Count; indexs++)
                            {
                                EditCourseCapsuleActivities Activity = lessonpod.Activities[indexs];

                                #region Set Lesson parameters                    
                                var activityParameters = new DynamicParameters();

                                activityParameters.Add("ip_course_capsule_id", CourseCapsuleID, DbType.Int32);
                                activityParameters.Add("ip_course_capsule_activity_id", Activity.CourseCapsuleActivityID, DbType.Int32);
                                activityParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                                activityParameters.Add("ip_activity_deleted", Activity.ActivityDeleted, DbType.Int32);
                                activityParameters.Add("ip_course_capsule_lessonpod_id", CourseCapsuleLessonPodID, DbType.Int32);
                                activityParameters.Add("ip_slide_id", Activity.SlideID, DbType.String);
                                activityParameters.Add("ip_slide_type", Activity.SlideType, DbType.String);
                                activityParameters.Add("ip_slide_name", Activity.SlideName, DbType.String);
                                activityParameters.Add("ip_is_selected", Activity.IsSelected, DbType.Int32);
                                activityParameters.Add("ip_activity_type", Activity.ActivityType, DbType.String);
                                activityParameters.Add("ip_min_score", Activity.MinScore, DbType.Int32);
                                activityParameters.Add("ip_min_time_min", Activity.MinTimeMin, DbType.Int32);
                                activityParameters.Add("ip_min_time_sec", Activity.MinTimeSec, DbType.Int32);
                                activityParameters.Add("ip_max_time_min", Activity.MaxTimeMin, DbType.Int32);
                                activityParameters.Add("ip_max_time_sec", Activity.MaxTimeSec, DbType.Int32);
                                activityParameters.Add("ip_follow_the_flow", Activity.FollowTheFlow, DbType.Int32);
                                activityParameters.Add("ip_auto_hint", Activity.AutoHint, DbType.Int32);
                                activityParameters.Add("ip_content_mode", Activity.ContentMode, DbType.String);
                                activityParameters.Add("ip_is_contained_view", Activity.IsContainedView, DbType.Boolean);
                                activityParameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsule, DbType.String);
                                activityParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                                activityParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                                activityParameters.Add("op_course_capsule_activity_id", DbType.Int64, direction: ParameterDirection.Output);
                                #endregion

                                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.EditCourseCapsuleActivity, activityParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                };
                BaseResponse response = new()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = CourseCapsuleMessage.EditCourseCapsule
                };
                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new BaseResponse
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
                await sqlTransaction.RollbackAsync();
                return new BaseResponse
                {
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
    }
}
