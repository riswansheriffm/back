
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Messages;
using BackPack.Library.Repositories.Interfaces.ClassLink;
using BackPack.Library.Requests.ClassLink;
using BackPack.Library.Responses.ClassLink;
using Dapper;
using KnomadixInfrastructure.AES256;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using BackPack.Library.Constants;

namespace BackPack.Library.Repositories.Repositories.ClassLink
{
    public class ClassLinkRepository : GenericRepository, IClassLinkRepository
    {
        #region ClassLinkAsync        
        public async Task<ClassLinkResponse> ClassLinkAsync(CreateClassLinkRequest request)
        {
            var dbConnection = DatabaseConnection();
            await dbConnection.OpenAsync();
            NpgsqlTransaction sqlTransaction;
            sqlTransaction = (NpgsqlTransaction)dbConnection!.BeginTransaction();

            try
            {
                #region Set Parameters
                var parameters = new DynamicParameters();
                parameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                parameters.Add("op_result", NpgsqlDbType.Refcursor, direction: ParameterDirection.Output);
                #endregion 

                var procResponse = await Task.FromResult(dbConnection!.Query(ServiceConstant.SchemaMasters + "get_classlink_data", parameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                var outRecordSet = parameters.Get<string>("op_result");
                string recordSetQuery = "FETCH ALL IN \"" + outRecordSet + "\"";
                var recordSetData = await Task.FromResult(dbConnection!.Query<ClassLinkResponseData>(recordSetQuery, sqlTransaction, commandType: CommandType.Text));

                var district = recordSetData.FirstOrDefault();

                if (district == null)
                {
                    return new ClassLinkResponse
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = ClassLinkMessage.InvalidDomainID,
                        Data = null
                    };
                }

                #region CreateClassLinkSchool
                string apiUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/schools?limit=10000";
                string bearerToken = district!.AccessToken!;

                JObject responseJson = await FetchSchoolDataAsync(apiUrl, bearerToken!);
                var schools = responseJson["orgs"]!;

                foreach (var school in schools!)
                {
                    string schoolName = (string)school["name"]!;
                    string schoolSourceId = (string)school["sourcedId"]!;

                    #region Set product parameters     
                    var schoolParameters = new DynamicParameters();
                    schoolParameters.Add("ip_school_name", schoolName, DbType.String);
                    schoolParameters.Add("ip_school_desc", schoolName, DbType.String);
                    schoolParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    schoolParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    schoolParameters.Add("ip_activity_desc", LogMessage.CreateSchool, DbType.String);
                    schoolParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    schoolParameters.Add("ip_source_id", schoolSourceId, DbType.String);
                    schoolParameters.Add("op_school_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    schoolParameters.Add("op_school_count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var Result = await Task.FromResult(dbConnection!.Query(ServiceConstant.SchemaMasters + "create_classlink_school", schoolParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                    int schoolId = schoolParameters.Get<int>("op_school_id");
                    int schoolCount = schoolParameters.Get<int>("op_school_count");
                    #endregion

                }
                #endregion

                #region CreateClassLinkSubject  
                string subjectUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/courses?limit=10000";

                JObject subjectResponseJson = await FetchSubjectDataAsync(subjectUrl, bearerToken!);
                var subjects = subjectResponseJson["courses"]!;

                foreach (var subject in subjects!)
                {
                    string subjectName = (string)subject["title"]!;
                    string subjectSourceID = (string)subject["sourcedId"]!;

                    #region Set product parameters     
                    var subjectParameters = new DynamicParameters();
                    subjectParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    subjectParameters.Add("ip_subject_name", subjectName, DbType.String);
                    subjectParameters.Add("ip_subject_desc", subjectName, DbType.String);
                    subjectParameters.Add("ip_grade_id", 169, DbType.Int32);
                    subjectParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    subjectParameters.Add("ip_activity_desc", LogMessage.CreateSubject, DbType.String);
                    subjectParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    subjectParameters.Add("ip_source_id", subjectName, DbType.String);
                    subjectParameters.Add("op_return_status_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var userResult = await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + "create_classlink_subject", subjectParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                    int ReturnStatusID = subjectParameters.Get<int>("op_return_status_id");
                    #endregion

                }
                #endregion

                #region CreateClassLinkCourse  
                string courseUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/classes?limit=10000";

                JObject courseResponseJson = await FetchClassDataAsync(courseUrl, bearerToken!);
                var courseDatas = courseResponseJson["classes"]!;

                foreach (var courseData in courseDatas!)
                {
                    string title = (string)courseData["title"]!;
                    string courceSourceID = (string)courseData["sourcedId"]!;
                    string subjectsourceID = (string)courseData["course"]!["sourcedId"]!;
                    string schoolSourceID = (string)courseData["school"]!["sourcedId"]!;

                    #region Set product parameters     
                    var subjectParameters = new DynamicParameters();
                    subjectParameters.Add("ip_course_name", title, DbType.String);
                    subjectParameters.Add("ip_course_desc", title, DbType.String);
                    subjectParameters.Add("ip_subjec_source_id", subjectsourceID, DbType.String);
                    subjectParameters.Add("ip_school_id", schoolSourceID, DbType.String);
                    subjectParameters.Add("ip_course_type", 0, DbType.Int32);
                    subjectParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                    subjectParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                    subjectParameters.Add("ip_activity_desc", LogMessage.CreateCourse, DbType.String);
                    subjectParameters.Add("ip_image_url", "https://classroom.knomadixapp.com/static/media/class-img-new.cc24dec9a866cb4ec3bc.jpeg", DbType.String);
                    subjectParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                    subjectParameters.Add("ip_source_id", courceSourceID, DbType.String);
                    subjectParameters.Add("op_course_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var userResult = await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + "create_classlink_course", subjectParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                    int ReturnStatus = subjectParameters.Get<int>("op_course_id");
                    #endregion

                }
                #endregion

                #region CreateClassLinkUser  
                string userUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/users?limit=10000";

                JObject userResponseJson = await FetchUserDataAsync(userUrl, bearerToken!);
                var userDatas = userResponseJson["users"]!;

                var userPasswordSalt = Hash.GetSecureSalt();
                var userPasswordHash = Hash.HashUsingPbkdf2("1234", userPasswordSalt);

                foreach (var userData in userDatas!)
                {

                    string userType = (string)userData["role"]!;
                    if (userType == "student" || userType == "teacher")
                    {
                        string LoginName = (string)userData["username"]!;
                        string userSourceID = (string)userData["sourcedId"]!;
                        string fName = (string)userData["givenName"]!;
                        string lName = (string)userData["familyName"]!;
                        string email = (string)userData["email"]!;
                        string phone = (string)userData["phone"]!;
                        var schoolSourceIDList = (userData["orgs"]!).Select(org => org["sourcedId"]?.ToString()).Where(sourcedId => !string.IsNullOrEmpty(sourcedId)).ToList();
                        string schoolSourceIDs = string.Join(",", schoolSourceIDList);

                        #region Set product parameters     
                        var userParameters = new DynamicParameters();
                        userParameters.Add("ip_login_name", LoginName, DbType.String);
                        userParameters.Add("ip_first_name", fName, DbType.String);
                        userParameters.Add("ip_last_name", lName, DbType.String);
                        userParameters.Add("ip_email_id", email, DbType.String);
                        userParameters.Add("ip_gmail_id", email, DbType.String);
                        userParameters.Add("ip_phone_no", phone, DbType.String);
                        userParameters.Add("ip_domain_name", district.DomainName, DbType.String);
                        userParameters.Add("ip_is_active", true, DbType.Boolean);
                        userParameters.Add("ip_is_primary", true, DbType.Boolean);
                        userParameters.Add("ip_password", userPasswordHash, DbType.String);
                        userParameters.Add("ip_password_salt", Convert.ToBase64String(userPasswordSalt).ToString(), DbType.String);
                        userParameters.Add("ip_role", userType, DbType.String);
                        userParameters.Add("ip_school_source_id", schoolSourceIDs, DbType.String);
                        userParameters.Add("ip_domain_id", request.DomainID, DbType.Int32);
                        userParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        userParameters.Add("ip_user_type_id", GlobalApplicationProperty.UserTypeID, DbType.Int32);
                        userParameters.Add("ip_activity_desc", LogMessage.CreateUser, DbType.String);
                        userParameters.Add("ip_source_id", userSourceID, DbType.String);
                        userParameters.Add("op_user_id", DbType.Int32, direction: ParameterDirection.Output);
                        userParameters.Add("op_domain_name", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);

                        var userResult = await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaUsers + "create_classlink_user", userParameters, commandType: CommandType.StoredProcedure));
                        int user_id = userParameters.Get<int>("op_user_id");
                    }
                    #endregion

                }
                #endregion

                #region MapTeacherandStudentToCourse  
                List<string> sourcedIds = await GetSourceIds(courseUrl, bearerToken!);

                foreach (var sourceId in sourcedIds)
                {
                    string teacherUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/classes/{sourceId}/teachers";
                    List<string> teacherSourcedIds = await GetTeacherData(teacherUrl, bearerToken);

                    string studentUrl = $"https://oneroster-proxy.classlink.io/proxy/v1p0/{(district!.ApplicationID)}/ims/oneroster/v1p1/classes/{sourceId}/students?limit=100000";
                    List<string> studentSourcedIds = await GetStudentData(studentUrl, bearerToken);

                    List<string> studentAndTeacherData = teacherSourcedIds.Concat(studentSourcedIds).ToList();

                    foreach (var userId in studentAndTeacherData)
                    {
                        #region Set product parameters     
                        var mapParameters = new DynamicParameters();
                        mapParameters.Add("ip_class_id", sourceId, DbType.String);
                        mapParameters.Add("ip_user_id", userId, DbType.String);
                        mapParameters.Add("ip_activity_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        mapParameters.Add("ip_modified_by", GlobalApplicationProperty.UserID, DbType.Int32);
                        mapParameters.Add("op_return_status", DbType.Int32, direction: ParameterDirection.Output);

                        var userResult = await Task.FromResult(dbConnection!.Execute(ServiceConstant.SchemaMasters + "map_student_and_teacher_to_course", mapParameters, sqlTransaction, commandType: CommandType.StoredProcedure));
                        int ReturnStatus = mapParameters.Get<int>("op_return_status");
                        #endregion
                    }
                }
                #endregion

                #region response
                var response = new ClassLinkResponse
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = ClassLinkMessage.ClassLinkCreated,
                    Data = district
                };
                #endregion

                sqlTransaction.Commit();
                return response;
            }
            catch (NpgsqlException ex)
            {
                sqlTransaction.Rollback();
                return new ClassLinkResponse
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
                return new ClassLinkResponse
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

        #region FetchSchoolDataAsync
        public async Task<JObject> FetchSchoolDataAsync(string apiUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(apiUrl);
                return JObject.Parse(response);
            }
        }
        #endregion

        #region FetchSubjectDataAsync 
        public async Task<JObject> FetchSubjectDataAsync(string subjectUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(subjectUrl);
                return JObject.Parse(response);
            }
        }
        #endregion

        #region FetchClassDataAsync 
        public async Task<JObject> FetchClassDataAsync(string courseUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(courseUrl);
                return JObject.Parse(response);
            }
        }
        #endregion

        #region FetchUserDataAsync 
        public async Task<JObject> FetchUserDataAsync(string userUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(userUrl);
                return JObject.Parse(response);
            }
        }
        #endregion

        #region GetSourceIds
        public async Task<List<string>> GetSourceIds(string courseUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(courseUrl);

                var jsonArray = JObject.Parse(response);

                var classes = jsonArray["classes"]?.ToObject<List<JObject>>();

                var sourceIds = classes!.Select(c => c["sourcedId"]?.ToString()).Where(id => id != null).ToList();

                return sourceIds!;
            }
        }
        #endregion

        #region GetTeacherData   
        public async Task<List<string>> GetTeacherData(string teacherUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(teacherUrl);

                var jsonArray = JObject.Parse(response);

                var users = jsonArray["users"]?.ToObject<List<JObject>>();

                var teacherSourcedIds = users!.Select(user => user["sourcedId"]?.ToString()).Where(id => id != null).ToList();

                return teacherSourcedIds!;
            }
        }

        #endregion 

        #region GetStudentData   
        public async Task<List<string>> GetStudentData(string studentUrl, string bearerToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                var response = await httpClient.GetStringAsync(studentUrl);

                var jsonArray = JObject.Parse(response);

                var users = jsonArray["users"]?.ToObject<List<JObject>>();

                var studentSourcedIds = users!.Select(user => user["sourcedId"]?.ToString()).Where(id => id != null).ToList();

                return studentSourcedIds!;
            }
        }

        #endregion

    }

}
