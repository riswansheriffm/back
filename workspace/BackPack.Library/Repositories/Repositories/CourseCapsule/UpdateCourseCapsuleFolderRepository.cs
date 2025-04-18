using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.CourseCapsule;
using BackPack.Library.Requests.CourseCapsule;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public class UpdateCourseCapsuleFolderRepository : GenericRepository, IUpdateCourseCapsuleFolderRepository
    {
        #region UpdateCourseCapsuleFolderAsync          
        public async Task<BaseResponse> UpdateCourseCapsuleFolderAsync(UpdateCourseCapsuleFolderRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            BaseResponse response = new();

            try
            {
                int folderCount = 0;
                #region Store users                
                for (int index = 0; index < request.Folders!.Count; index++)
                {
                    UpdateCourseCapsuleFolderReorderList folder = request.Folders[index];

                    #region Set user parameters                    
                    var parameters = new DynamicParameters();
                    parameters.Add("ip_course_capsule_id", request.CourseCapsuleID, DbType.Int32);
                    parameters.Add("ip_course_capsule_folder_id", folder.CourseCapsuleFolderID, DbType.Int32);
                    parameters.Add("ip_display_order", folder.DisplayOrder, DbType.Int32);
                    parameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    parameters.Add("ip_activity_desc", LogMessage.UpdateCourseCapsuleFolderReorder, DbType.String);
                    parameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    parameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);
                    #endregion

                    await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.UpdateCourseCapsuleFolderReorder, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                    int statudId = parameters.Get<int>("op_return_status");
                    if (statudId == 1)
                    {
                        folderCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion

                if (folderCount > 0 && request.Folders.Count == folderCount)
                {
                    await sqlTransaction.CommitAsync();
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = CourseCapsuleMessage.UpdateCourseCapsuleLicense;
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                }

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
