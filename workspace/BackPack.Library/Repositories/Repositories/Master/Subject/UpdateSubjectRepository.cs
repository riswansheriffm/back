using BackPack.Dependency.Library.Messages;
using BackPack.Library.Repositories.Interfaces.Master.Subject;
using BackPack.Library.Requests.Master.Subject;
using BackPack.Dependency.Library.Responses;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Messages;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.Master.Subject
{
    public class UpdateSubjectRepository : GenericRepository, IUpdateSubjectRepository
    {
        #region UpdateSubjectAsync        
        public async Task<BaseResponse> UpdateSubjectAsync(UpdateSubjectRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set user parameters   
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_subject_id", request.SubjectID, DbType.Int32);
                userParameters.Add("ip_grade_id", request.GradeID, DbType.Int32);
                userParameters.Add("ip_subject_name", request.SubjectName, DbType.String);
                userParameters.Add("ip_subject_desc", request.SubjectDesc, DbType.String);
                userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                userParameters.Add("ip_activity_desc", LogMessage.UpdateSubject, DbType.String);
                userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                userParameters.Add("op_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + ProcedureConstant.UpdateSubject, userParameters, commandType: CommandType.StoredProcedure);
                int userResult = userParameters.Get<int>("op_status_id");
                #endregion

                #region Response  
                if (userResult > 0)
                {
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.SuccessID,
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        StatusMessage = MasterMessage.UpdateSubject
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        MessageID = CommonMessage.FailID,
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = MasterMessage.UpdateCreationFail
                    };
                }
                #endregion
            }
            catch (NpgsqlException ex)
            {
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
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
