using BackPack.Library.Helpers.LessonPod;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class LessonPodReDistributionRepository : GenericRepository, ILessonPodReDistributionRepository
    {
        #region LessonPodReDistributionAsync
        public async Task<BaseResponse> LessonPodReDistributionAsync(LessonPodReDistributionRequest request)
        {
            bool IsMapped = false;
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Get student IDs
                string StudentIDs = "";
                if (request.WhomToDistribute.ToLower().ToString() == "selectstudents")
                {
                    StudentIDs = string.Join(",", request.StudentIDs);
                }
                else
                {
                    StudentIDs = await LessonPodDistributionCommon.DistributionStudentList(request.WhomToDistribute, request.CourseID, string.Join(",", request.GroupIDs), dbConnection, sqlTransaction);
                }

                #region No student response
                if (StudentIDs == "")
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = 1,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.NoStudents,
                        ExceptionType = CommonMessage.ExceptionTypeNormal
                    };
                }
                #endregion

                #endregion

                #region Audit log
                //DistributionAuditLogResponse auditLogRequest = new()
                //{
                //    LessonUnitDistID = request.LessonUnitDistID,
                //    UserID = request.AuthorID,
                //    DistributionTypeID = 3,
                //    ActivityDescription = LessonPodMessage.LessonPodReDistributionAuditLog + request.LessonName
                //};

                //await LessonPodDistributionCommon.SaveLessonPodDistributionAuditLogAsync(auditLogRequest, dbConnection, sqlTransaction);
                #endregion

                #region Remove existing distributed data
                var removeDataParameters = new DynamicParameters();
                removeDataParameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
                removeDataParameters.Add("op_return_status", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.DeleteLessonUnitDistributionByLessonUnitDistId, removeDataParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int ReturnStatus = removeDataParameters.Get<int>("op_return_status");

                #region Fail response
                if (ReturnStatus == 0)
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = 1,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.LessonPodReDistributionFail,
                        ExceptionType = CommonMessage.ExceptionTypeNormal
                    };
                }
                #endregion

                #endregion

                #region Lesson pod distribution creation

                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_dist_id", request.LessonUnitDistID, DbType.Int32);
                parameters.Add("ip_author_id", request.AuthorID, DbType.Int32);
                parameters.Add("ip_lesson_name", request.LessonName, DbType.String);
                parameters.Add("ip_lesson_desc", request.LessonDesc, DbType.String);
                parameters.Add("ip_flag_visible_to_parent", (request.FlagVisibleToParent == 1), DbType.Boolean);
                parameters.Add("ip_course_id", request.CourseID, DbType.Int32);
                parameters.Add("ip_whom_to_distribute", request.WhomToDistribute, DbType.String);
                parameters.Add("ip_group_ids", string.Join(",", request.GroupIDs), DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_lesson_unit_dist_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_lesson_json", dbType: DbType.String, direction: ParameterDirection.Output, size: 40000000);
                parameters.Add("op_lesson_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_course_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.RecreateLessonUnitDistribution, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                int LessonUnitDistID = parameters.Get<int>("op_lesson_unit_dist_id");
                var lessonJson = parameters.Get<string>("op_lesson_json");
                var lessonID = parameters.Get<int>("op_lesson_id");

                #region Fail response
                if (lessonJson == "")
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = 1,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.LessonPodReDistributionFail,
                        ExceptionType = CommonMessage.ExceptionTypeNormal
                    };
                }
                #endregion

                #region Redistribution
                List<LessonPodSlideListResponse> slideList = [];
                JObject jobject = JObject.Parse(lessonJson);
                int ParentContentID = 0;
                int PublishedContentID = 0;
                int ContentID = 0;
                for (int slideIndex = 0; slideIndex < request.LessonUnitSlides.Count; slideIndex++)
                {
                    string SlideID = "";
                    string ParentSlideID = "";
                    bool isAvailable = false;

                    do
                    {
                        SlideID = request.LessonUnitSlides[slideIndex].SlideID;
                        ParentSlideID = request.LessonUnitSlides[slideIndex].ParentSlideID;
                        bool isParentAvailable = false;

                        if (SlideID != "")
                        {
                            if (ParentSlideID == "")
                            {
                                isAvailable = false;
                                ParentContentID = 0;
                            }

                            foreach (var activity in slideList)
                            {
                                if (activity.SlideID == ParentSlideID)
                                {
                                    isAvailable = false;
                                    isParentAvailable = true;
                                    ParentContentID = activity.ContentID;
                                    break;
                                }
                            }

                            if (ParentSlideID != "" && !isParentAvailable)
                            {
                                for (int parentIndex = 0; parentIndex < request.LessonUnitSlides.Count; parentIndex++)
                                {
                                    if (request.LessonUnitSlides[parentIndex].SlideID == ParentSlideID)
                                    {
                                        LessonPodSlideRequest parentSlide = request.LessonUnitSlides[parentIndex];
                                        request.LessonUnitSlides[parentIndex] = request.LessonUnitSlides[slideIndex];
                                        request.LessonUnitSlides[slideIndex] = parentSlide;
                                        isAvailable = true;
                                        break;
                                    }
                                }
                            }
                        }
                    } while (isAvailable);

                    LessonPodSlideListResponse slide = new();
                    LessonPodSlideRequest lessonUnitSlideRequest = request.LessonUnitSlides[slideIndex];

                    SlideID = lessonUnitSlideRequest.SlideID;
                    ParentSlideID = lessonUnitSlideRequest.ParentSlideID;

                    if (SlideID != "")
                    {
                        JObject objNavItemsById = (JObject)jobject["present"]!["navItemsById"]![SlideID]!;
                        JObject objViewToolbarsById = (JObject)jobject["present"]!["viewToolbarsById"]![SlideID]!;
                        string ContentName = DistributionCommonHelper.JObjectToString(objViewToolbarsById, "viewName");
                        string SlideType = DistributionCommonHelper.JObjectToString(objNavItemsById, "tempname");
                        bool isContainedView = DistributionCommonHelper.JObjectToBoolean(objNavItemsById, "containedview");

                        SaveContentResponse saveContentResponse = new()
                        {
                            ParentContentID = ParentContentID,
                            LessonUnitDistID = LessonUnitDistID,
                            PublishedContentID = PublishedContentID,
                            LessonID = lessonID,
                            LoginID = request.AuthorID,
                            CourseID = request.CourseID,
                            DomainID = request.DomainID,
                            FlagVisibleToParent = (request.FlagVisibleToParent == 1),
                            IsContainedView = isContainedView,
                            SlideID = SlideID,
                            ContentName = ContentName,
                            LessonPodType = request.LessonPodType
                        };

                        if (SlideType != "")
                        {
                            #region Smart Label                                
                            if (SlideType == "Smart Label")
                            {
                                LessonPodSlideResponse lessonUnitSlideResponse = SmartLabelHelper.SmartLabelJson(jobject, SlideID);
                                lessonUnitSlideResponse.AppName = "SmartLabel";

                                ContentID = await LessonPodDistributionCommon.SaveContentAsync(saveContentResponse, lessonUnitSlideRequest, lessonUnitSlideResponse, dbConnection, sqlTransaction);

                                #region Error response                                    
                                if (ContentID == 0)
                                {
                                    await sqlTransaction.RollbackAsync();

                                    return new BaseResponse()
                                    {
                                        MessageID = CommonMessage.FailID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                        ExceptionType = CommonMessage.ExceptionTypeNormal
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Smart Tile
                            if (SlideType == "Smart Tile")
                            {
                                LessonPodSlideResponse lessonUnitSlideResponse = SmartTileHelper.SmartTileJson(jobject, SlideID);
                                lessonUnitSlideResponse.AppName = "SmartTiles";

                                ContentID = await LessonPodDistributionCommon.SaveContentAsync(saveContentResponse, lessonUnitSlideRequest, lessonUnitSlideResponse, dbConnection, sqlTransaction);

                                #region Error response                                    
                                if (ContentID == 0)
                                {
                                    await sqlTransaction.RollbackAsync();
                                    return new BaseResponse()
                                    {
                                        MessageID = CommonMessage.FailID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                        ExceptionType = CommonMessage.ExceptionTypeNormal
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Smart Paper
                            if (SlideType == "Smart Paper")
                            {
                                LessonPodSlideResponse lessonUnitSlideResponse = SmartPaperHelper.SmartPaperJson(jobject, SlideID);
                                lessonUnitSlideResponse.AppName = "SmartPaper";

                                ContentID = await LessonPodDistributionCommon.SaveContentAsync(saveContentResponse, lessonUnitSlideRequest, lessonUnitSlideResponse, dbConnection, sqlTransaction);

                                #region Error response                                    
                                if (ContentID == 0)
                                {
                                    await sqlTransaction.RollbackAsync();
                                    return new BaseResponse()
                                    {
                                        MessageID = CommonMessage.FailID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                        ExceptionType = CommonMessage.ExceptionTypeNormal
                                    };
                                }
                                #endregion

                                if (lessonUnitSlideResponse.SmartPaperInputControls!.Count > 0 || lessonUnitSlideResponse.SmartPaperStrokeReports!.Count > 0)
                                {
                                    int ControlResponse = await LessonPodDistributionCommon.SaveSmartPaperControlAsync(ContentID, lessonUnitSlideResponse.SmartPaperInputControls, lessonUnitSlideResponse.SmartPaperReplayRectangleReports!, dbConnection, sqlTransaction);

                                    #region Error response                                    
                                    if (ControlResponse == 1)
                                    {
                                        await sqlTransaction.RollbackAsync();
                                        return new BaseResponse()
                                        {
                                            MessageID = CommonMessage.FailID,
                                            Success = false,
                                            StatusCode = StatusCodes.Status400BadRequest,
                                            StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                            ExceptionType = CommonMessage.ExceptionTypeNormal
                                        };
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Smart Slide
                            if (SlideType == "Smart Slide")
                            {
                                LessonPodSlideResponse lessonUnitSlideResponse = SmartSlideHelper.SmartSlideJson(jobject, SlideID);
                                lessonUnitSlideResponse.AppName = "SmartSlide";

                                ContentID = await LessonPodDistributionCommon.SaveContentAsync(saveContentResponse, lessonUnitSlideRequest, lessonUnitSlideResponse, dbConnection, sqlTransaction);

                                #region Error response                                    
                                if (ContentID == 0)
                                {
                                    await sqlTransaction.RollbackAsync();
                                    return new BaseResponse()
                                    {
                                        MessageID = CommonMessage.FailID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                        ExceptionType = CommonMessage.ExceptionTypeNormal
                                    };
                                }
                                #endregion

                                int SlideControlResponse = await LessonPodDistributionCommon.SaveSmartSlideControlAsync(ContentID, lessonUnitSlideResponse.SmartSlideInputControls!, dbConnection, sqlTransaction);

                                #region Error response                                    
                                if (SlideControlResponse == 1)
                                {
                                    await sqlTransaction.RollbackAsync();
                                    return new BaseResponse()
                                    {
                                        MessageID = CommonMessage.FailID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.LessonPodDistributionFail,
                                        ExceptionType = CommonMessage.ExceptionTypeNormal
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Map students with content
                            await LessonPodDistributionCommon.SaveStudentsWithContentAsync(ContentID, StudentIDs, request.FlagVisibleToParent, dbConnection, sqlTransaction);
                            #endregion
                        }
                        slide.ContentID = ContentID;
                        slide.SlideID = SlideID;
                        slide.ParentSlideID = ParentSlideID;
                        slideList.Add(slide);
                    }
                }

                #region Map students with distributed lesson
                if (LessonUnitDistID > 0)
                {
                    int RecordCount = await LessonPodDistributionCommon.SaveStudentsWithDistributedLessonAsync(LessonUnitDistID, StudentIDs, request, dbConnection, sqlTransaction);
                    if (RecordCount > 0) IsMapped = true;
                    DistributionAuditLogResponse auditLogRequest = new()
                    {
                        LessonUnitDistID = LessonUnitDistID,
                        UserID = request.AuthorID,
                        DistributionTypeID = 1,
                        ActivityDescription = LessonPodMessage.LessonPodDistributionAuditLog + request.LessonName
                    };

                    await LessonPodDistributionCommon.SaveLessonPodDistributionAuditLogAsync(auditLogRequest, dbConnection, sqlTransaction);
                }
                #endregion

                #endregion

                #endregion

                #region Response
                if (IsMapped)
                {
                    await sqlTransaction.CommitAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.LessonPodReDistributionSuccess
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.LessonPodReDistributionFail,
                        ExceptionType = CommonMessage.ExceptionTypeNormal
                    };
                }
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
                {
                    MessageID = 3,
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
                    MessageID = 3,
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
