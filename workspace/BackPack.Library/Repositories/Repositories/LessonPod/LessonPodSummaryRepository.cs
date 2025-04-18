using BackPack.Dependency.Library.Messages;
using BackPack.Library.Responses.LessonPod;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Constants;
using Npgsql;

namespace BackPack.Library.Repositories.Repositories.LessonPod
{
    public class LessonPodSummaryRepository : GenericRepository, ILessonPodSummaryRepository
    {
        #region LessonPodSummaryForAStudentAsync
        public async Task<LessonPodSummaryForAStudentResponse> LessonPodSummaryForAStudentAsync(int StudentID, int LessonUnitDistID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            LessonPodSummaryForAStudentResponse response = new();

            try
            {
                #region Set parameters            
                var parameters = new DynamicParameters();
                parameters.Add("StudentID", StudentID, DbType.Int32);
                parameters.Add("LessonUnitDistID", LessonUnitDistID, DbType.Int32);
                #endregion

                var data = await Task.FromResult(dbConnection!.Query<LessonPodSummaryForAStudentData>(ServiceConstant.SchemaLessonpods + "GetLessonUnitSummaryForAStudent", parameters, commandType: CommandType.StoredProcedure));

                #region Response
                response.Data = data.ToList();
                response.Success = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.StatusMessage = CommonMessage.ReadMessage;
                #endregion

                return response;
            }
            catch (NpgsqlException ex)
            {
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusMessage = CommonMessage.ExceptionMessage;
                response.ExceptionType = CommonMessage.ExceptionTypeSqlException;
                response.ExceptionMessage = ex.Message + " : " + ex.StackTrace;

                return response;
            }
            catch (Exception ex)
            {
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
