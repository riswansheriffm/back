namespace BackPack.Tenant.Library.Constants
{
    public static class ServiceConstant
    {
        #region User type
        public const string SuperAdmin = "SuperAdmin";
        #endregion

        #region Service name
        public const string SuperAdminLoginService = "SuperAdminLogin";
        public const string RefreshTokenService = "RefreshToken";
        public const string ResetPasswordService = "ResetPassword";
        public const string ServiceStatusService = "ServiceStatus";
        #endregion

        public const string TokenType = "Bearer";

        #region Schema name
        public const string SchemaMasters = "masters.";
        #endregion

        #region Settings
        public const string JwtSettings = "JWTSettings";
        public const string Issuer = "Issuer";
        public const string Audience = "Audience";
        public const string Key = "Key";
        public const string TokenExpirationInMinutes = "TokenExpirationInMinutes";
        public const string RefreshTokenExpirationInDays = "RefreshTokenExpirationInDays";
        #endregion
    }
}
