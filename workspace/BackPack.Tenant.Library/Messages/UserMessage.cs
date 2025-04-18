namespace BackPack.Tenant.Library.Messages
{
    public static class UserMessage
    {
        #region Login

        #region Swagger
        public const string SuperAdminLoginSummary = "Super Admin login";
        public const string SuperAdminLoginDescription = "Super Admin login";
        #endregion

        public const string UserNotFound = "Invalid username. Please try again.";
        public const string InvalidPassword = "Incorrect password. Please try again.";
        public const string DomainNotFound = "Invalid school district. Please try again.";
        public const string LockedAccount = "Your Knomadix account has been locked. Please reset your password.";
        #endregion

        #region Refresh token
        public const string RefreshTokenSummary = "Generate new access and refresh token.";
        public const string RefreshTokenDescription = "Generate new access and refresh token based on the existing refresh token and user.";

        public const string RefreshTokenFail = "Invalid refresh token.";
        #endregion

        #region GetSuperAdmin
        public const string GetSuperAdminSummary = "SuperAdmin Details";
        public const string GetSuperAdminDescription = "SuperAdmin Details In Dashboard";
        #endregion 
    }
}
