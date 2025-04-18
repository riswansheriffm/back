using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;
using BackPack.Dependency.Library.Helpers;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class AllSlideTemplateRepository : GenericRepository, IAllSlideTemplateRepository
    {
        #region AllSlideTemplateAsync
        public async Task<AllSlideTemplateResponse> AllSlideTemplateAsync(int DomainID, int LoginID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();
            AllSlideTemplateResponse response = new();
            AllSlideTemplateData responseData = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_login_id", LoginID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_user_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.QueryAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.GetAllStudioSlideTemplates, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = GlobalHelper.StringToString(outRecordSet);
                var recordSetData = await dbConnection.QueryAsync<SlideTemplateData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outUserRecordSet = parameters.Get<string>("op_user_record_set");
                string userRecordSetQuery = GlobalHelper.StringToString(outUserRecordSet);
                var userRecordSetData = await dbConnection.QueryAsync<SlideTemplateData>(userRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                #region Read db response
                var listThemes = recordSetData.ToList();
                var listGallery = userRecordSetData.ToList();
                #endregion


                #region Response data
                responseData.Themes = listThemes;
                responseData.Gallery = listGallery;
                
                var responseResult = new AllSlideTemplateResult
                {
                    GetAllStudioSlideTemplatesResult = responseData
                };
                #endregion

                #region Response
                response.Data = responseResult;
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
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
