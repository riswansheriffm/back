using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class CreateLessonPodSlideTemplateRepository : GenericRepository, ICreateLessonPodSlideTemplateRepository
    {
        #region CreateLessonPodSlideTemplateAsync
        public async Task<CreateLessonPodSlideTemplateResponse> CreateLessonPodSlideTemplateAsync(CreateLessonPodSlideTemplateRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_slide_template_id", request.SlideTemplateID, DbType.Int32);
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("ip_template_name", request.TemplateName, DbType.String);
                parameters.Add("ip_access_type", request.AccessType, DbType.String);
                parameters.Add("ip_question_color", request.QuestionColor, DbType.String);
                parameters.Add("ip_answer_color", request.AnswerColor, DbType.String);
                parameters.Add("ip_font_style", request.FontStyle, DbType.String);
                parameters.Add("ip_question_font_size", request.QuestionFontSize, DbType.String);
                parameters.Add("ip_background", request.Background, DbType.String);
                parameters.Add("ip_answer_font_size", request.AnswerFontSize, DbType.String);
                parameters.Add("ip_bold", request.Bold, DbType.Boolean);
                parameters.Add("ip_italic", request.Italic, DbType.Boolean);
                parameters.Add("ip_opacity", request.Opacity, DbType.String);
                parameters.Add("ip_selection_color", request.SelectionColor, DbType.String);
                parameters.Add("ip_answer_fill_color", request.AnswerFillColor, DbType.String);
                parameters.Add("ip_question_fill_color", request.QuestionFillColor, DbType.String);
                parameters.Add("ip_activity_desc", (request.SlideTemplateID == 0) ? LogMessage.CreateSlideTemplate : LogMessage.UpateSlideTemplate, DbType.String);
                parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("op_slide_template_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("op_return_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.SaveStudioSlideTemplate, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                #region Response
                int SlideTemplateID = parameters.Get<int>("op_slide_template_id");
                int ReturnID = parameters.Get<int>("op_return_id");

                if (ReturnID == 1)
                {
                    await sqlTransaction.CommitAsync();

                    return new CreateLessonPodSlideTemplateResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        SlideTemplateID = SlideTemplateID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = LessonPodMessage.CreateStudioSlideTemplateSuccess
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();

                    return new CreateLessonPodSlideTemplateResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        SlideTemplateID = SlideTemplateID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = LessonPodMessage.CreateStudioSlideTemplateFail
                    };
                }

                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new CreateLessonPodSlideTemplateResponse()
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

                return new CreateLessonPodSlideTemplateResponse()
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
