using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.District;
using BackPack.Library.Repositories.Interfaces.School;
using BackPack.Library.Requests.District;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.District
{
    public class DistrictStatusRepository : GenericRepository, IDistrictStatusRepository
    {
        #region DistrictStatus       
        public async Task<BaseResponse> DistrictStatusAsync(DistrictStatusRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            BaseResponse response = new();

            try
            {                
                #region Set user parameters  
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_domain_id", request.ID, DbType.Int32);
                userParameters.Add("ip_activity_by", request.ActivityBy, DbType.Int32);
                userParameters.Add("ip_user_type_id", 3, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.DeleteDomain, DbType.String);
                userParameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "delete_district", userParameters, commandType: CommandType.StoredProcedure);
                int userResult = userParameters.Get<int>("op_status_id");
                #endregion

                #region Response
                if (userResult > 0)
                {
                    response.MessageID = CommonMessage.SuccessID;
                    response.Success = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    response.StatusMessage = DistrictMessage.DistrcitDeleted;
                }
                else
                {
                    response.MessageID = CommonMessage.FailID;
                    response.Success = false;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusMessage = CommonMessage.InternalServerErrorMessage;
                }

                return response;
                #endregion
            }
            catch (NpgsqlException ex)
            {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
