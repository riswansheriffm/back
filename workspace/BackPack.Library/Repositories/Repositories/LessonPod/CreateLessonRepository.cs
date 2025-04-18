using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class CreateLessonRepository : GenericRepository, ICreateLessonRepository
    {
        #region CreateLessonAsync        
        public async Task<BaseResponse> CreateLessonAsync(CreateLessonRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            BaseResponse response = new();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_lesson_name", request.LessonName, DbType.String);
                userParameters.Add("ip_lesson_desc", request.LessonDesc, DbType.String);
                userParameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateLesson, DbType.String);                
                userParameters.Add("ip_image_url", request.ImageURL, DbType.String);
                userParameters.Add("ip_chapter_id", request.ChapterID, DbType.Int32);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_return_lesson_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateLesson, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int LessonFolderID = userParameters.Get<int>("op_return_lesson_id");
                #endregion 

                if (LessonFolderID == 0)
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = LessonPodMessage.LessonCreationFail;
                }
                else if (LessonFolderID == -1)
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.DuplicateID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = LessonPodMessage.CreateLessonDuplicate;
                }

                else if (LessonFolderID > 0 && request.Tags!.Count > 0)
                {
                    foreach (var tag in request.Tags)
                    {
                        string TagName = tag.ToString();

                        #region Set user parameters  
                        var parameters = new DynamicParameters();
                        parameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                        parameters.Add("ip_lesson_id", LessonFolderID, DbType.Int32);
                        parameters.Add("ip_tag_name", TagName, DbType.String);
                        parameters.Add("ip_activity_desc", LogMessage.CreateSubjectTag, DbType.String);
                        parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        parameters.Add("op_lesson_tag_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateSubjectTagAndMapping, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                        int LessonTagID = parameters.Get<int>("op_lesson_tag_id");
                        #endregion

                        if (LessonTagID > 0)
                        {
                            response.MessageID = CommonMessage.SuccessID;
                            response.Success = true;
                            response.StatusCode = StatusCodes.Status201Created;
                            response.StatusMessage = LessonPodMessage.CreateLessonCapsule;
                        }
                        else
                        {
                            await sqlTransaction.RollbackAsync();
                            response.MessageID = CommonMessage.FailID;
                            response.Success = false;
                            response.StatusCode = StatusCodes.Status500InternalServerError;
                            response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                        }
                    }
                }
                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                response.MessageID = CommonMessage.ExceptionID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeSqlException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;
                return response;
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
                response.MessageID = CommonMessage.ExceptionID;
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;
                return response;
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
