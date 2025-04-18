using BackPack.Library.Constants;
using BackPack.Library.Helpers.LessonPod;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using BackPack.Library.Messages;
using Npgsql;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class CreateLessonPodRepository : GenericRepository, ICreateLessonPodRepository
    {
        #region CreateLessonPodAsync
        public async Task<CreateLessonPodResponse> CreateLessonPodAsync(CreateLessonPodRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Check version
                JObject jobject = JObject.Parse(request.LessonJson!);
                string VersionNumber = DistributionCommonHelper.JObjectToString(jobject, "present", "version");
                if (VersionNumber == "")
                {
                    await sqlTransaction.RollbackAsync();
                    return new CreateLessonPodResponse()
                    {
                        MessageID = CommonMessage.InvalidParameterID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage,
                        ExceptionType = CommonMessage.ExceptionTypeValidation,
                        ExceptionMessage = LessonPodMessage.CreateLessonPodVersionError
                    };
                }
                #endregion

                #region Save lesson pod

                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_lesson_unit_id", request.LessonUnitID, DbType.Int32);
                parameters.Add("ip_lesson_id", request.LessonID, DbType.Int32);
                parameters.Add("ip_lesson_name", request.LessonName, DbType.String);
                parameters.Add("ip_lesson_desc", request.LessonDesc, DbType.String);
                parameters.Add("ip_author_id", GlobalApplicationProperty.UserID, DbType.Int32);                
                parameters.Add("ip_access_type", "Private", DbType.String);
                parameters.Add("ip_version_number", VersionNumber, DbType.String);
                parameters.Add("ip_lesson_json", request.LessonJson, DbType.String);
                parameters.Add("ip_activity_desc", (request.LessonUnitID == 0) ? LogMessage.CreateLessonpod : LogMessage.UpdateLessonpod, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("op_lesson_unit_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.CreateLessonUnit, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #region Response
                int LessonUnitID = parameters.Get<int>("op_lesson_unit_id");

                if (LessonUnitID == 0)
                {
                    await sqlTransaction.RollbackAsync();

                    return new CreateLessonPodResponse()
                    {
                        LessonUnitID = LessonUnitID,
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.CreateLessonPodUpdationFail,
                        ExceptionMessage = LessonPodMessage.CreateLessonPodUpdationFail
                    };
                }
                #endregion

                #region Save lesson pod activity
                if (LessonUnitID > 0)
                {
                    List<LessonPodSlideListResponse> listSlide = LessonPodHelper.LessonPodSlideList(request.LessonJson!);
                    int ReturnID = 0;
                    foreach (var slide in listSlide)
                    {
                        string SlideType = slide.SlideType;
                        if (SlideType != "")
                        {
                            LessonPodSlideResponse lessonPodSlideResponse = new();

                            #region Smart Label
                            if (SlideType == ServiceConstant.SmartLabel)
                            {
                                lessonPodSlideResponse = SmartLabelHelper.SmartLabelJson(jobject, slide.SlideID);
                                lessonPodSlideResponse.AppName = ServiceConstant.AppSmartLabel;
                                ReturnID = await LessonPodCommon.SaveLessonPodActivityAsync(LessonUnitID, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                                #region Response
                                if (ReturnID != 1)
                                {
                                    await sqlTransaction.RollbackAsync();

                                    return new CreateLessonPodResponse()
                                    {
                                        LessonUnitID = LessonUnitID,
                                        MessageID = CommonMessage.InvalidParameterID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.CreateLessonPodFail,
                                        ExceptionMessage = LessonPodMessage.CreateLessonPodFail
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Smart Tile
                            if (SlideType == ServiceConstant.SmartTile)
                            {
                                lessonPodSlideResponse = SmartTileHelper.SmartTileJson(jobject, slide.SlideID);
                                lessonPodSlideResponse.AppName = ServiceConstant.AppSmartTile;
                                ReturnID = await LessonPodCommon.SaveLessonPodActivityAsync(LessonUnitID, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                                #region Response
                                if (ReturnID != 1)
                                {
                                    await sqlTransaction.RollbackAsync();

                                    return new CreateLessonPodResponse()
                                    {
                                        LessonUnitID = LessonUnitID,
                                        MessageID = CommonMessage.InvalidParameterID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.CreateLessonPodFail,
                                        ExceptionMessage = LessonPodMessage.CreateLessonPodFail
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Smart Paper
                            if (SlideType == ServiceConstant.SmartPaper)
                            {
                                lessonPodSlideResponse = SmartPaperHelper.SmartPaperJson(jobject, slide.SlideID);
                                lessonPodSlideResponse.AppName = ServiceConstant.AppSmartPaper;
                                ReturnID = await LessonPodCommon.SaveLessonPodActivityAsync(LessonUnitID, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                                #region Response
                                if (ReturnID != 1)
                                {
                                    await sqlTransaction.RollbackAsync();

                                    return new CreateLessonPodResponse()
                                    {
                                        LessonUnitID = LessonUnitID,
                                        MessageID = CommonMessage.InvalidParameterID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.CreateLessonPodFail,
                                        ExceptionMessage = LessonPodMessage.CreateLessonPodFail
                                    };
                                }
                                #endregion
                            }
                            #endregion

                            #region Smart Slide
                            if (SlideType == ServiceConstant.SmartSlide)
                            {
                                lessonPodSlideResponse = SmartSlideHelper.SmartSlideJson(jobject, slide.SlideID);
                                lessonPodSlideResponse.AppName = ServiceConstant.AppSmartSlide;
                                ReturnID = await LessonPodCommon.SaveLessonPodActivityAsync(LessonUnitID, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                                #region Response
                                if (ReturnID != 1)
                                {
                                    await sqlTransaction.RollbackAsync();

                                    return new CreateLessonPodResponse()
                                    {
                                        LessonUnitID = LessonUnitID,
                                        MessageID = CommonMessage.InvalidParameterID,
                                        Success = false,
                                        StatusCode = StatusCodes.Status400BadRequest,
                                        StatusMessage = LessonPodMessage.CreateLessonPodFail,
                                        ExceptionMessage = LessonPodMessage.CreateLessonPodFail
                                    };
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }

                    #region Response
                    await sqlTransaction.CommitAsync();

                    return new CreateLessonPodResponse()
                    {
                        LessonUnitID = LessonUnitID,
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = (request.LessonUnitID == 0) ? LessonPodMessage.CreateLessonPodSuccess : LessonPodMessage.UpdateLessonPodSuccess
                    };
                    #endregion
                }
                #endregion

                #endregion

                #region Fail response
                await sqlTransaction.RollbackAsync();

                return new CreateLessonPodResponse()
                {
                    MessageID = CommonMessage.ErrorID,
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ExceptionType = CommonMessage.ExceptionTypeFail,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                };
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new CreateLessonPodResponse()
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

                return new CreateLessonPodResponse()
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
    }
}
