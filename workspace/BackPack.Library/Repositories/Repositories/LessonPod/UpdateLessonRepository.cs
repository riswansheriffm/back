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
    public class UpdateLessonRepository : GenericRepository, IUpdateLessonRepository
    {
        #region UpdateLessonAsync        
        public async Task<BaseResponse> UpdateLessonAsync(UpdateLessonRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            string TagName = "";
            int SubjectTagID = 0;
            BaseResponse response = new();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_lesson_name", request.LessonName, DbType.String);
                userParameters.Add("ip_lesson_desc", request.LessonDesc, DbType.String);
                userParameters.Add("ip_lesson_id", request.LessonID, DbType.Int32); 
                userParameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LessonPodMessage.UpdateLesson, DbType.String);                
                userParameters.Add("ip_image_url", request.ImageURL, DbType.String);
                userParameters.Add("ip_chapter_id", request.ChapterID, DbType.Int32);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_return_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateLesson, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int ReturnStatusID = userParameters.Get<int>("op_return_status_id");
                #endregion 

                if (ReturnStatusID == 2)
                {
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = LessonPodMessage.InvalidChapter;
                    await sqlTransaction.RollbackAsync();
                    return response;
                }
                else if (ReturnStatusID <= 0) 
                {
                    response.MessageID = CommonMessage.DuplicateID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = LessonPodMessage.UpdateLessonDuplicate;
                    await sqlTransaction.RollbackAsync();
                    return response;
                }

                else if (ReturnStatusID == 1 && request.Tags!.Count > 0)
                {
                    foreach (var tag in request.Tags)
                    {
                        TagName = tag.ToString();

                        #region Set user parameters  
                        var parameters = new DynamicParameters();
                        parameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                        parameters.Add("ip_lesson_id", request.LessonID, DbType.Int32);
                        parameters.Add("ip_tag_name", TagName, DbType.String);
                        parameters.Add("ip_activity_desc", LogMessage.CreateSubjectTag, DbType.String);
                        parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        parameters.Add("op_lesson_tag_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + ProcedureConstant.CreateSubjectTagAndMapping, parameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                        int LessonTagID = parameters.Get<int>("op_lesson_tag_id");
                        #endregion
                    }
                }

                if (ReturnStatusID == 1 && request.DeletedTags!.Count > 0)
                {
                    foreach (var tag in request.DeletedTags)
                    {
                        SubjectTagID = tag;

                        #region Set user parameters  
                        var parameters = new DynamicParameters();
                        parameters.Add("ip_lesson_id", request.LessonID, DbType.Int32);
                        parameters.Add("ip_subject_tag_id", SubjectTagID, DbType.Int32);
                        parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        parameters.Add("ip_activity_desc", LogMessage.DeleteSubjectTag, DbType.String);
                        parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);

                        await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + ProcedureConstant.DeleteTagMapping, parameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                        #endregion
                    }                    
                }

                response.MessageID = CommonMessage.SuccessID;
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.StatusMessage = LessonPodMessage.UpdateLessonCapsule;
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
