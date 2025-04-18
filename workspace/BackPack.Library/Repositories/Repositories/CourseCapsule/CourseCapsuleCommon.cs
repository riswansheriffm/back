
using BackPack.Library.Constants;
using BackPack.Library.Responses.CourseCapsule;
using BackPack.Library.Responses.LessonPod.Distribution;
using Dapper;
using Npgsql;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.CourseCapsule
{
    public static class CourseCapsuleCommon
    {
        #region DeleteCourseCapsuleFolderAsync
        public static async Task<int> DeleteCourseCapsuleFolderAsync(int courseCapsuleId, SaveCourseCapsuleFolderResponseData folderRequest, NpgsqlConnection dbConnection, NpgsqlTransaction sqlTransaction)
        {
            int response = 0;
            foreach (var folder in folderRequest.SaveCourseCapsuleFolder!)
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_course_capsule_id", courseCapsuleId, DbType.Int32);
                parameters.Add("ip_course_capsule_folder_id", folder.CourseCapsuleFolderID, DbType.Int32);
                parameters.Add("op_return_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + ProcedureConstant.DeleteCourseCapsuleFolder, parameters, sqlTransaction, commandType: CommandType.StoredProcedure);

                response = parameters.Get<int>("op_return_id");
                if (response != 1)
                {
                    break;
                }
            }

            return response;
        }
        #endregion
    }
}
