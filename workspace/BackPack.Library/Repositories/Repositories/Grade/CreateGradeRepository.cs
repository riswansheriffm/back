using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Grade;
using BackPack.Library.Requests.Grade;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Grade
{
    public class CreateGradeRepository : GenericRepository, ICreateGradeRepository
    {
        #region CreateGradeAsync        
        public async Task<BaseResponse> CreateGradeAsync(CreateGradeRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction = await dbConnection.BeginTransactionAsync();

            try
            {
                #region Set user parameters  
                var userParameters = new DynamicParameters();                
                userParameters.Add("ip_grade_name", request.GradeName, DbType.String);
                userParameters.Add("ip_grade_desc", request.GradeDesc, DbType.String);
                userParameters.Add("ip_domain_id", request.DistrictID, DbType.Int32);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.CreateGrade, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_return_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.CreateGrade, userParameters, sqlTransaction, commandType: CommandType.StoredProcedure);
                int ReturnStatusID = userParameters.Get<int>("op_return_status_id");
                #endregion

                #region Response  

                if (ReturnStatusID == 0)
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = GradeMessage.GradeCreationFail
                    };

                }
                else if (ReturnStatusID == 1)
                {
                    await sqlTransaction.CommitAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = GradeMessage.CreateGrade
                    };
                }
                else if (ReturnStatusID == 2)
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.DuplicateID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = GradeMessage.GradeCreationDuplicate
                    };
                }
                else
                {
                    await sqlTransaction.RollbackAsync();
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.ErrorID,
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        StatusMessage = CommonMessage.InternalServerErrorMessage
                    };
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
