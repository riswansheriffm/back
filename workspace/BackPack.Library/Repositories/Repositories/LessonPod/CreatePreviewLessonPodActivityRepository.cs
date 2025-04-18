using BackPack.Library.Constants;
using BackPack.Library.Helpers.LessonPod;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod.Distribution;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using BackPack.Library.Messages;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class CreatePreviewLessonPodActivityRepository : GenericRepository, ICreatePreviewLessonPodActivityRepository
    {
        #region CreatePreviewLessonPodActivityAsync
        public async Task<BaseResponse> CreatePreviewLessonPodActivityAsync(CreatePreviewLessonPodActivityRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Delete lesson pod preview activities based on user

                #region Set parameters
                var deleteParameters = new DynamicParameters();
                deleteParameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.DeleteLessonUnitPreviewActivities, deleteParameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #endregion

                #region Save lesson pod preview activity
                JObject jobject = JObject.Parse(request.LessonJson!);
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
                            ReturnID = await LessonPodCommon.SaveLessonPodPreviewActivityAsync(request, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                            #region Response
                            if (ReturnID != 1)
                            {
                                await sqlTransaction.RollbackAsync();

                                return new BaseResponse()
                                {
                                    MessageID = CommonMessage.InvalidParameterID,
                                    Success = false,
                                    StatusCode = StatusCodes.Status400BadRequest,
                                    StatusMessage = LessonPodMessage.CreatePreviewLessonPodActivityFail
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
                            ReturnID = await LessonPodCommon.SaveLessonPodPreviewActivityAsync(request, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                            #region Response
                            if (ReturnID != 1)
                            {
                                await sqlTransaction.RollbackAsync();

                                return new BaseResponse()
                                {
                                    MessageID = CommonMessage.InvalidParameterID,
                                    Success = false,
                                    StatusCode = StatusCodes.Status400BadRequest,
                                    StatusMessage = LessonPodMessage.CreatePreviewLessonPodActivityFail
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
                            ReturnID = await LessonPodCommon.SaveLessonPodPreviewActivityAsync(request, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                            #region Response
                            if (ReturnID != 1)
                            {
                                await sqlTransaction.RollbackAsync();

                                return new BaseResponse()
                                {
                                    MessageID = CommonMessage.InvalidParameterID,
                                    Success = false,
                                    StatusCode = StatusCodes.Status400BadRequest,
                                    StatusMessage = LessonPodMessage.CreatePreviewLessonPodActivityFail
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
                            ReturnID = await LessonPodCommon.SaveLessonPodPreviewActivityAsync(request, slide, lessonPodSlideResponse, dbConnection, sqlTransaction);

                            #region Response
                            if (ReturnID != 1)
                            {
                                await sqlTransaction.RollbackAsync();

                                return new BaseResponse()
                                {
                                    MessageID = CommonMessage.InvalidParameterID,
                                    Success = false,
                                    StatusCode = StatusCodes.Status400BadRequest,
                                    StatusMessage = LessonPodMessage.CreatePreviewLessonPodActivityFail
                                };
                            }
                            #endregion
                        }
                        #endregion
                    }
                }

                #region Response

                await sqlTransaction.CommitAsync();

                return new BaseResponse()
                {
                    MessageID = CommonMessage.SuccessID,
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = LessonPodMessage.CreatePreviewLessonPodActivitySuccess
                };
                #endregion

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
