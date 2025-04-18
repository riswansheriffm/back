
using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.DBLog;
using BackPack.Library.Requests.DBLog;
using Dapper;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.DBLog
{
    public class DBLogRepository : GenericRepository, IDBLogRepository
    {
        #region CreateRequestAsync
        public async Task<bool> CreateRequestAsync(DBLogRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_service_log_uuid", request.ServiceLogID, DbType.Guid);
                parameters.Add("ip_login_id", request.LoginID, DbType.Int32);
                parameters.Add("ip_login_type", request.LoginType, DbType.String);
                parameters.Add("ip_service_type", request.ServiceType, DbType.String);
                parameters.Add("ip_service_name", request.ServiceName, DbType.String);
                parameters.Add("ip_service_method_name", request.ServiceMethodName, DbType.String);
                parameters.Add("ip_service_request", request.ServiceRequest, DbType.String);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLogs + ProcedureConstant.CreateServiceLog, parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region UpdateRequestAsync
        public async Task<bool> UpdateRequestAsync(DBLogRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_service_log_uuid", request.ServiceLogID, DbType.Guid);
                parameters.Add("ip_service_response", request.ServiceResponse, DbType.String);
                parameters.Add("ip_service_status", request.ServiceStatus, DbType.String);
                parameters.Add("ip_log_message", request.LogMessage, DbType.String);
                parameters.Add("ip_exception_type", request.ExceptionType, DbType.String);
                parameters.Add("ip_exception_message", request.ExceptionMessage, DbType.String);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLogs + ProcedureConstant.UpdateServiceLog, parameters, commandType: CommandType.StoredProcedure);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
