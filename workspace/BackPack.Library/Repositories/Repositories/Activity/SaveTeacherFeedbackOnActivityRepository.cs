using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Activity;
using BackPack.Library.Requests.Activity;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Activity
{
    public class SaveTeacherFeedbackOnActivityRepository : GenericRepository, ISaveTeacherFeedbackOnActivityRepository
    {
        #region SaveTeacherFeedbackOnActivityAsync
        public async Task<BaseResponse> SaveTeacherFeedbackOnActivityAsync(TeacherFeedbackOnActivityRequest request)
        {
            var response = new BaseResponse();
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", request.AuthorID, DbType.Int32);
                parameters.Add("ip_student_id", request.StudentID, DbType.Int32);
                parameters.Add("ip_content_id", request.ContentID, DbType.Int32);
                parameters.Add("ip_rework", request.Rework, DbType.Int32);
                parameters.Add("ip_feedback", request.Feedback, DbType.String);
                parameters.Add("ip_grade", request.Grade, DbType.Int32);
                parameters.Add("ip_remarks", request.Remarks, DbType.String);
                parameters.Add("ip_teacher_recording", request.TeacherRecording, DbType.String);
                parameters.Add("ip_teacher_ink", request.TeacherInk, DbType.String);
                parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                parameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaStudents + ProcedureConstant.SaveTeacherFeedbackOnActivity, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                var statusID = parameters.Get<int>("op_status_id");

                #region Response
                if (statusID == 1)
                {
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = ActivityMessage.TeacherFeedbackRework;

                    await sqlTransaction.CommitAsync();

                    return response;
                }

                if (statusID == 2)
                {
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = ActivityMessage.TeacherFeedbackSaved;

                    await sqlTransaction.CommitAsync();

                    return response;
                }

                if (statusID == 3)
                {
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusMessage = ActivityMessage.TeacherFeedbackNotSubmited;

                    await sqlTransaction.RollbackAsync();

                    return response;
                }
                else
                {
                    response.MessageID = CommonMessage.ErrorID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;

                    await sqlTransaction.RollbackAsync();

                    return response;
                }
                #endregion
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();

                return new BaseResponse()
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

                return new BaseResponse()
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
