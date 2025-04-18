namespace BackPack.APIGateway.Constants
{
    public static class ServiceConstant
    {
        #region User type
        public const string SuperAdmin = "SuperAdmin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
        #endregion

        #region Service name
        public const string SuperAdminLoginService = "SuperAdminLogin";
        public const string TeacherLoginService = "TeacherLogin";
        public const string StudentLoginService = "StudentLogin";
        public const string RefreshTokenService = "RefreshToken";
        public const string ActivateUserAccountService = "ActivateUserAccount";
        public const string ResetPasswordService = "ResetPassword";
        public const string CreatePasswordService = "CreatePassword";
        public const string ServiceStatusService = "ServiceStatus";
        public const string ServiceCreateDocument = "CreateDocument";
        public const string ServiceGetAllDocument = "GetAllDocument";
        #endregion
    }
}
