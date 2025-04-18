using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Dashboard;
using BackPack.Library.Responses.Dashboard;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Constants;
using Npgsql;
using NpgsqlTypes;

namespace BackPack.Library.Repositories.Repositories.Dashboard
{
    public class ClassRoomUpcomingAssignmentRepository : GenericRepository, IClassRoomUpcomingAssignmentRepository
    {
        #region ClassRoomUpcomingAssignmentsResponseAsync        
        public async Task<ClassRoomUpcomingAssignmentsResponse> ClassRoomUpcomingAssignmentsResponseAsync(int AuthorID, string AssignmentDate)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("ip_author_id", AuthorID, DbType.Int32);
                parameters.Add("ip_assignment_date", AssignmentDate, DbType.String);
                parameters.Add("op_record_set", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion

                var procResponse = await dbConnection.QueryAsync(ServiceConstant.SchemaUsers + "get_cr_upcoming_assignments", parameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                var outRecordSet = parameters.Get<string>("op_record_set");
                string recordSetQuery = "";
                if (outRecordSet != "")
                {
                    recordSetQuery = $"FETCH ALL IN \"{outRecordSet}\"";
                }
                var recordSetData = await dbConnection.QueryAsync<ClassRoomUpcomingAssignmentsData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text);

                var responseData = new ClassRoomUpcomingAssignmentsDataResult
                {
                    GetCRUpcomingAssignmentsResult = recordSetData.ToList()
                };

                #region Response
                ClassRoomUpcomingAssignmentsResponse response = new()
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
                return new ClassRoomUpcomingAssignmentsResponse
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
                return new ClassRoomUpcomingAssignmentsResponse
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
    }
}
