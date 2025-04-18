namespace BackPack.Tenant.Library.Requests
{
    public class TokenRequest
    {
        public int TokenExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; }
        public int UserID { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string TenantID { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string DBConnection { get; set; } = string.Empty.ToString();
        public string UserType { get; set; } = string.Empty;
        public string TokenUserType { get; set; } = string.Empty;
        public string TokenHash { get; set; } = string.Empty;
        public string TokenSalt { get; set; } = string.Empty;
        public string TokenUserID { get; set; } = string.Empty;
        public string TokenLoginName { get; set; } = string.Empty;
        public string TokenDomainID { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
