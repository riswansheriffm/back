using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Responses.CourseCapsule;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;
using BackPack.Dependency.Library.Responses;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class SaveCourseCapsuleFolderRepository : GenericRepository, ISaveCourseCapsuleFolderRepository
    {
        #region SaveCourseCapsuleFolderAsync         
        public async Task<SaveCourseCapsuleFolderResponse> SaveCourseCapsuleFolderAsync(SaveCourseCapsuleFolderRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            SaveCourseCapsuleFolderResponse response = new();
            List<int> listCourseCapsuleID = [];

            try
            {
                #region Store users                
                for (int index = 0; index < request.Folder?.Count; index++)
                {
                    SaveCourseCapsuleFolderListObject Class = request.Folder[index];

                    #region Set user parameters                    
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_login_id", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);                    
                    userParameters.Add("ip_course_capsule_folder_id", Class.CourseCapsuleFolderID, DbType.Int32);
                    userParameters.Add("ip_folder_name", Class.FolderName, DbType.String);
                    userParameters.Add("ip_folder_desc", Class.FolderDesc, DbType.String);
                    userParameters.Add("ip_activity_desc", (Class.CourseCapsuleFolderID > 0) ? LogMessage.UpdateCourseCapsuleFolder : LogMessage.CreateCourseCapsuleFolder, DbType.String);
                    userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    userParameters.Add("op_course_capsule_folder_id", DbType.Int32, direction: ParameterDirection.Output);
                    userParameters.Add("op_return_id", DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.CreateCourseCapsuleFolder, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                    int ReturnID = userParameters.Get<int>("op_return_id");
                    int ReturnCourseCapsuleFolderID = userParameters.Get<int>("op_course_capsule_folder_id");

                    if (ReturnID != 1)
                    {
                        await sqlTransaction.RollbackAsync();
                        return new SaveCourseCapsuleFolderResponse
                        {
                            MessageID = CommonMessage.FailID,
                            Success = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            StatusMessage = CourseCapsuleMessage.InvalidSaveCourseCapsule
                        };
                    }
                    listCourseCapsuleID.Add(ReturnCourseCapsuleFolderID);
                }
                #endregion
                string CouseCapsuleFolderIDs = "";
                CouseCapsuleFolderIDs = string.Join(",", listCourseCapsuleID);

                #region Set user parameters                     
                var parameters = new DynamicParameters();
                parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                parameters.Add("ip_course_capsule_folder_ids", CouseCapsuleFolderIDs, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllCourseCapsuleFolderForDelete, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SaveCourseCapsuleFolder>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var details = recordSetData;

                var responseData = new SaveCourseCapsuleFolderResponseData
                {
                    SaveCourseCapsuleFolder = details.ToList()
                };

                int returnId = 1;
                if (responseData.SaveCourseCapsuleFolder.Count > 0)
                {
                    returnId = await CourseCapsuleCommon.DeleteCourseCapsuleFolderAsync(request.CourseCapsuleID, responseData, dbConnection, sqlTransaction);
                }                 

                if (returnId == 1)
                {
                    response = new SaveCourseCapsuleFolderResponse
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = CourseCapsuleMessage.SaveCourseCapsuleFolderSuccess
                    };

                    await sqlTransaction.CommitAsync();
                    return response;
                }
                else
                {
                    response = new SaveCourseCapsuleFolderResponse
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
                    };

                    await sqlTransaction.RollbackAsync();
                    return response;
                }
                
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new SaveCourseCapsuleFolderResponse()
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
                return new SaveCourseCapsuleFolderResponse()
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
