using BackPack.Dependency.Library.Messages;
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Responses.Master.Course;
using BackPack.Library.Responses.Master.Document;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.Master
{
    public class DocumentRepository : GenericRepository, IDocumentRepository
    {
        #region GetAllDocumentAsync          
        public async Task<DocumentResponse> GetAllDocumentAsync(float[] EmbeddingVector, int Limit)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction;
            sqlTransaction = (NpgsqlTransaction)dbConnection.BeginTransaction();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();

                parameters.Add("ip_embedding", EmbeddingVector, dbType: DbType.Object);
                parameters.Add("ip_limit", Limit, DbType.Int32);
                parameters.Add("op_result", dbType: DbType.Object, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + "get_document", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_result");
                string recordSetQuery = "FETCH ALL IN \"" + outRecordSet + "\"";
                var recordSetData = await dbConnection.QueryAsync<GetAllDocumentResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new DocumentResponseData
                {
                    GetAllDocumentResult = recordSetData.ToList()
                };

                #region Response
                DocumentResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion
                sqlTransaction.Commit();
                return response;
            }
            catch (NpgsqlException ex)
            {
                sqlTransaction.Rollback();
                return new DocumentResponse
                {
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
                return new DocumentResponse
                {
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
