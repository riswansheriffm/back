
namespace BackPack.Library.Messages
{
    public static class UserMessage
    {
        #region Login

        #region Swagger
        public const string TeacherLoginSummary = "Teacher login";
        public const string TeacherLoginDescription = "Teacher login";
        public const string StudentLoginSummary = "Student login";
        public const string StudentLoginDescription = "Student login";
        #endregion

        public const string UserNotFound = "Invalid username. Please try again.";
        public const string InvalidPassword = "Incorrect password. Please try again.";
        public const string DomainNotFound = "Invalid school district. Please try again.";
        public const string LockedAccount = "Your Knomadix account has been locked. Please reset your password.";
        #endregion

        #region User upload
        public const string UserUploadSummary = "Upload Teacher and Student";
        public const string UserUploadDescription = "File upload for Teacher and Student users";

        public const string UserCreateSummary = "Create Users";
        public const string UserCreateDescription = "create users";

        public const string UserDeleteSummary = "Delete Users";
        public const string UserDeleteDescription = "Delete users";
        public const string UserUpdateSummary = "Update Users";
        public const string UserUpdateDescription = "Update users";

        public const string InvalidDomain = "Invalid Domain";
        public const string SchoolNotFound = "Invalid School";
        public const string InvalidDomainSchool = "Invalid Domain / School";
        public const string UnknownError = "Something went wrong";
        public const string DuplicateTeacher = "Duplicate Teacher";
        public const string DuplicateStudent = "Duplicate Student";
        public const string InvalidClass = "Invalid class";
        public const string TeacherAlreadyMapped = "Teacher already mapped with the class";
        public const string StudentAlreadyMapped = "Student already mapped with the class";
        public const string UserUploadSuccess = "User Data uploaded successfully.";
        public const string UserCreated = "User Created successfully.";
        public const string UserDeleted = "User Deleted successfully.";
        public const string UserUpdated = "User Updated successfully.";
        public const string MigratedUserPasswordCreated = "Migrated user password created successfully.";
        #endregion

        #region User
        public const string SuperUserCreateSummary = "Create super user";
        public const string SuperUserCreateDescription = "create super user";

        public const string StudentDeleteSummary = "Delete student";
        public const string StudentDeleteDescription = "Delete student";
        #endregion

        #region User account activation
        public const string ActivateUserAccountSummary = "User account activation";
        public const string ActivateUserAccountDescription = "User account activation";

        public const string ActivateUserAccountSucess = "Success";
        public const string ActivateUserAccountFail = "No Matching record found";
        public const string ActivateUserAccountAlreadyActivated = "User account already activated.";
        #endregion

        #region Reset credential
        public const string ResetCredentialSummary = "Reset user credential";
        public const string ResetCredentialDescription = "Reset credential";

        public const string ResetCredentialSucess = "Success";
        public const string ResetCredentialFail = "No Matching record found";
        #endregion

        #region Refresh token
        public const string RefreshTokenSummary = "Generate new access and refresh token.";
        public const string RefreshTokenDescription = "Generate new access and refresh token based on the existing refresh token and user.";

        public const string RefreshTokenFail = "Invalid refresh token.";
        #endregion

        #region Student dashboard
        public const string StudentDashboardSummary = "Student dashboard";
        public const string StudentDashboardDescription = "Student dashboard details";
        #endregion

        #region Student upcoming assignments
        public const string StudentDashboardUpcomingSummary = "Upcoming assignments";
        public const string StudentDashboardUpcomingDescription = "Upcoming assignments based on student";
        #endregion

        #region GetCRTeacherDashboard
        public const string ClassRoomDashboardSummary = "Classroom dashboard";
        public const string ClassRoomDashboardDescription = "Classroom dashboard details";
        #endregion

        #region GetCRTeacherDashboard
        public const string ClassRoomUpcomingAssignmentSummary = "Upcoming assignments";
        public const string ClassRoomUpcomingAssignmentDescription = "Upcoming assignments based on teacher";
        #endregion

        #region GetUser
        public const string UserSummary = "Get User Details";
        public const string UserDescription = "Get User Details By ID";
        #endregion

        #region GetAllUser
        public const string AllUserSummary = "Get All User Details";
        public const string AllUserDescription = "Get All User Details By ID";
        #endregion

        #region GetAllDomainUser
        public const string AllDomainUserSummary = "Get All Domain User Details";
        public const string AllDomainUserDescription = "Get All Domain User Details By ID";
        #endregion

        #region GetAllTeacherByClass
        public const string GetAllTeacherByClassSummary = "Get All Teacher By Class Details";
        public const string GetAllTeacherByClassDescription = "Get All Teacher By Class Details";
        #endregion

        #region GetSuperAdmin
        public const string GetSuperAdminSummary = "SuperAdmin Details";
        public const string GetSuperAdminDescription = "SuperAdmin Details In Dashboard";
        #endregion 

        #region GetDistrictAdmin
        public const string GetDistrictAdminSummary = "DistrictAdmin Details";
        public const string GetDistrictAdminDescription = "DistrictAdmin Details In Dashboard";
        #endregion

        #region GetCurriculumAdmin
        public const string GetCurriculumAdminSummary = "CurriculumAdmin Details";
        public const string GetCurriculumAdminDescription = "CurriculumAdmin Details In Dashboard";
        #endregion  

        #region SchoolAdminDashboard
        public const string SchoolAdminDashboardSummary = "SchoolAdmin Details";
        public const string SchoolAdminDashboardDescription = "SchoolAdmin Details In Dashboard";
        #endregion  

        public const string UpdateStudentSummary = "Update Student";
        public const string UpdateStudentDescription = "Update Student";
        public const string UpdateStudent = "Update Student Successfully.";
        public const string UpdateStudentDuplicate = "DuplicateKey";

        public const string AuthUserNotFound = "User not found.";
        public const string AuthenticateLoginSummary = "Authenticate login";
        public const string AuthenticateLoginDescription = "Authenticate login";

        #region GetSuperCA
        public const string SuperCaUserSummary = "Get User Details";
        public const string SuperCaUserDescription = "Get User Details By domain id";
        #endregion
    }
}
 