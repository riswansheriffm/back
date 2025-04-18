using BackPack.Library.Constants;
using BackPack.Library.Repositories.Interfaces.Global;
using Dapper;
using System.Data;

namespace BackPack.Library.Repositories.Repositories.Global
{
    public class GlobalRepository : GenericRepository, IGlobalRepository
    {
        #region CheckDomainByID
        public async Task<int> CheckDomainByID(int DomainID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("op_out_domain_count", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "check_if_domain_exists", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("op_out_domain_count");
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region CheckDomainByName
        public async Task<int> CheckDomainByName(string domainName)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {                
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_name", domainName, DbType.String);
                parameters.Add("op_domain_count", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "check_if_domain_name_exists", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("op_domain_count");
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region CheckSchoolByID
        public async Task<int> CheckSchoolByID(int SchoolID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpSchoolCount, DbType.Int64, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "check_if_school_exists", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>(ProcedureConstant.OpSchoolCount);
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region CheckDomainAndSchoolByID
        public async Task<int> CheckDomainAndSchoolByID(int DomainID, int SchoolID)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", DomainID, DbType.Int32);
                parameters.Add("ip_school_id", SchoolID, DbType.Int32);
                parameters.Add(ProcedureConstant.OpSchoolCount, DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaMasters + "check_if_domain_school_exists", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>(ProcedureConstant.OpSchoolCount);
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion        

        #region CheckCourseCapsuleByID
        public async Task<int> CheckCourseCapsuleByID(int DomainID, int SubjectID, int CourseCapsuleID, string CourseCapsuleName)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters                
                var userParameters = new DynamicParameters();
                userParameters.Add("ip_domain_id", DomainID, DbType.Int32);
                userParameters.Add("ip_subject_id", SubjectID, DbType.Int32);
                userParameters.Add("ip_course_capsule_id", CourseCapsuleID, DbType.Int32);
                userParameters.Add("ip_course_capsule_name", CourseCapsuleName, DbType.String);
                userParameters.Add("op_course_capsule_count", DbType.Int32, direction: ParameterDirection.Output);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaLessonpods + "check_if_course_capsule_exists", userParameters, commandType: CommandType.StoredProcedure);

                return userParameters.Get<int>("op_course_capsule_count");
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion

        #region GetLoginNameByID
        public async Task<string> GetLoginNameByID(int userID, string userType)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();

            try
            {
                #region Set parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_user_id", userID, DbType.Int32);
                parameters.Add("ip_user_type", userType, DbType.String);
                parameters.Add("op_return_login_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);
                #endregion

                await dbConnection.ExecuteAsync(ServiceConstant.SchemaUsers + "get_login_name_by_id", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("op_return_login_name");
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                await dbConnection.DisposeAsync();
            }
        }
        #endregion
    }
}
