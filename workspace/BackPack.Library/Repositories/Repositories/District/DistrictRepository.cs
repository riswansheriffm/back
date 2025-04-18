using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.District;
using BackPack.Library.Responses.District;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using BackPack.MessageContract.Library;
using Npgsql;
using NpgsqlTypes;
using BackPack.Library.Responses.School;

namespace BackPack.Library.Repositories.Repositories.District
{
    public class DistrictRepository : GenericRepository, IDistrictRepository
    {
        #region GetDistrictAsync        
        public async Task<GetDomainAcceptedEvent> GetDistrictAsync(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                parameters.Add("op_user_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + "get_district", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = "";
                if (outRecordSet != "")
                {
                    recordSetQuery = $"FETCH ALL IN \"{outRecordSet}\"";
                }

                var recordSetData = await dbConnection.QueryAsync<GetDistrictResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);
                var outUserRecordSet = parameters.Get<string>("op_user_record_set");
                string userRecordSetQuery = "";
                if (outUserRecordSet != "")
                {
                    userRecordSetQuery = $"FETCH ALL IN \"{outUserRecordSet}\"";
                }

                var userRecordSetData = await dbConnection.QueryAsync<DistrictObject>(userRecordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var district = recordSetData;
                var details = userRecordSetData;

                var responseData = new DistrictResponseData
                {
                    GetDistrictResult = new GetDistrictResult
                    {
                        Desc = district?.FirstOrDefault()?.Desc,
                        Name = district?.FirstOrDefault()?.Name,
                        ID = district?.FirstOrDefault()?.ID ?? 0,
                        StreetAddress = district?.FirstOrDefault()?.StreetAddress,
                        State = district?.FirstOrDefault()?.State,
                        AccessType = district?.FirstOrDefault()?.AccessType,
                        City = district?.FirstOrDefault()?.City,
                        Zip = district?.FirstOrDefault()?.Zip,
                        MaxStudents = district?.FirstOrDefault()?.MaxStudents ?? 0,
                        MaxTeachers = district?.FirstOrDefault()?.MaxTeachers ?? 0,
                        SourceID = district?.FirstOrDefault()?.SourceID,
                        ApplicationID = district?.FirstOrDefault()?.ApplicationID,
                        AccessToken = district?.FirstOrDefault()?.AccessToken,
                        Source = district?.FirstOrDefault()?.Source,
                        UO = details.FirstOrDefault()
                    }
                };
                #region Response
                GetDomainAcceptedEvent response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new GetDomainAcceptedEvent
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
                await sqlTransaction.RollbackAsync();
                return new GetDomainAcceptedEvent
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
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();
            }
        }

        #endregion

        #region GetAllDistrictAsync        
        public async Task<GetAllDistrictResponse> GetAllDistrictAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                var data = await dbConnection!.QueryMultipleAsync("GetDistricts", commandType: CommandType.StoredProcedure);
                {
                    var alldistrict = await data.ReadAsync<GetAllDistrictsResult>();

                    var responseData = new AllDistrictsResponseData
                    {
                        GetAllDistrictsResults = alldistrict.ToList()
                    };

                    #region Response
                    GetAllDistrictResponse response = new()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = CommonMessage.ReadMessage,
                        Data = new List<AllDistrictsResponseData> { responseData }
                    };
                    #endregion

                    return response;
                }
            }
            catch (NpgsqlException ex)
            {
                return new GetAllDistrictResponse
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
                return new GetAllDistrictResponse
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
                await dbConnection.DisposeAsync();
            }
        }

        #endregion

        #region GetAllPublicActiveDomainsAsync        
        public async Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveDomainsAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                var data = await dbConnection!.QueryMultipleAsync("GetAllPublicActiveDomains", commandType: CommandType.StoredProcedure);
                {
                    var domains = await data.ReadAsync<GetAllPublicActiveDomainsResult>();

                    var responseData = new AllPublicActiveDomainsResultResponseData
                    {
                        GetAllPublicActiveDomainsResult = domains.ToList()
                    };

                    #region Response
                    AllPublicActiveDomainsResultResponse response = new()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = CommonMessage.ReadMessage,
                        Data = new List<AllPublicActiveDomainsResultResponseData> { responseData }
                    };
                    #endregion

                    return response;
                }
            }
            catch (NpgsqlException ex)
            {
                return new AllPublicActiveDomainsResultResponse
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
                return new AllPublicActiveDomainsResultResponse
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
                await dbConnection.DisposeAsync();
            }
        }

        #endregion

        #region GetAllSubjectsByDomainAsync        
        public async Task<AllSubjectsByDomainResponse> GetAllSubjectsByDomainAsync(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {                
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaMasters + "get_all_subjects_by_domain", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = "";
                if (outRecordSet != "")
                {
                    recordSetQuery = $"FETCH ALL IN \"{outRecordSet}\"";
                }

                var recordSetData = await dbConnection.QueryAsync<GetAllSubjectsByDomainResult>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);


                var responseData = new AllSubjectsByDomainResponseData
                {
                    GetAllSubjectsByDomainResult = recordSetData.ToList()
                };

                #region Response
                AllSubjectsByDomainResponse response = new()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = CommonMessage.ReadMessage,
                    Data = responseData
                };
                #endregion

                await sqlTransaction.CommitAsync();
                return response;
            }
            catch (NpgsqlException ex)
            {
                await sqlTransaction.RollbackAsync();
                return new AllSubjectsByDomainResponse
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
                await sqlTransaction.RollbackAsync();
                return new AllSubjectsByDomainResponse
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
                await sqlTransaction.DisposeAsync();
                await dbConnection.DisposeAsync();
            }
        }

        #endregion

        #region GetAllActiveDomainsAsync        
        public async Task<GetAllActiveDomainsResponse> GetAllActiveDomainsAsync()
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                var data = await dbConnection!.QueryMultipleAsync("GetAllActiveDomains", commandType: CommandType.StoredProcedure);
                {
                    var domains = await data.ReadAsync<GetAllPublicActiveDomainsResult>();

                    var responseData = new GetAllActiveDomainsResponseData
                    {
                        GetAllActiveDomainsResult = domains.ToList()
                    };

                    #region Response
                    GetAllActiveDomainsResponse response = new()
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = CommonMessage.ReadMessage,
                        Data = new List<GetAllActiveDomainsResponseData> { responseData }
                    };
                    #endregion

                    return response;
                }
            }
            catch (NpgsqlException ex)
            {
                return new GetAllActiveDomainsResponse
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
                return new GetAllActiveDomainsResponse
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
                await dbConnection.DisposeAsync();
            }
        }

        #endregion
    }
}
