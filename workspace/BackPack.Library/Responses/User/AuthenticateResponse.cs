using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class AuthenticateResponse : ReadBaseResponse
    {
        public TokenResponse Token { get; set; } = new TokenResponse();
        public AuthenticateUserData? Data { get; set; }
    }

    public class AuthenticateUserData
    {
        public int DistrictID { get; set; }
        public string? DomainName { get; set; } = null;
        public int SchoolID { get; set; }
        public string? Role { get; set; } = null;
        public string? FName { get; set; } = null;
        public string? LName { get; set; }
        public Boolean PrimaryFlag { get; set; }
        public int LoginID { get; set; }
        public string? LoginName { get; set; } = null;
        public string? FullName { get; set; } = null;
        public string? UserID { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? Url { get; set; } = null;
        public string? PartnerID { get; set; } = null;
        public string? SubPartnerID { get; set; } = null;
        public string? AdministratorSecret { get; set; } = null;
        public string? UserSecret { get; set; } = null;
        public int LockOut { get; set; }
    }
}
