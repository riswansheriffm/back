using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Master.Document;
using BackPack.Library.Requests.Master.Document;
using Dapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.Master.Document
{
    public class CreateDocumentRepository : GenericRepository, ICreateDocumentRepository
    {
        #region CreateDocumentAsync        
        public async Task<BaseResponse> CreateDocumentAsync(DocumentRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction;
            sqlTransaction = (NpgsqlTransaction)dbConnection.BeginTransaction();
            BaseResponse response = new();

            try
            {
                #region Store users                
                for (int index = 0; index < request.documents?.Count; index++)
                {
                    DocumentList documents = request.documents[index];

                    #region Set user parameters  
                    var userParameters = new DynamicParameters();
                    userParameters.Add("ip_document_id", documents.title, DbType.String);
                    userParameters.Add("ip_embedding", documents.embedding, DbType.Object);
                    userParameters.Add("ip_content", documents.content, DbType.String);
                    userParameters.Add("ip_page", documents.page, DbType.Int32);
                    userParameters.Add("ip_source", documents.source, DbType.String);
                    userParameters.Add("op_return_code", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var userResult = await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "create_document", userParameters, commandType: CommandType.StoredProcedure);
                    #endregion

                    int ReturnStatus = userParameters.Get<int>("op_return_code");

                    #region Response 
                    if (ReturnStatus != 1)
                    {
                        sqlTransaction.Rollback();
                        response.MessageID = CommonMessage.FailID;
                        response.Success = false;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                        return response;
                    }
                    #endregion
                }
                #endregion

                sqlTransaction.Commit();
                response.MessageID = CommonMessage.SuccessID;
                response.Success = true;
                response.StatusCode = StatusCodes.Status201Created;
                response.StatusMessage = MasterMessage.CreateDocument;

                return response;
            }
            catch (NpgsqlException ex)
            {
                sqlTransaction.Rollback();
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
                sqlTransaction.Rollback();
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
                sqlTransaction.Dispose();
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
