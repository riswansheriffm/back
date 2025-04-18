using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class UserResponse : ReadBaseResponse
    {
        public UserResponseData Data { get; set; } = new();
    }
    public class UserResponseData
    {
        public GetUserResult GetUserResult { get; set; } = new();
    }
    public class GetUserResult
    {
        public string? ID { get; set; } = string.Empty;
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? LoginName { get; set; } = string.Empty;
        public string? PhoneNo { get; set; } = string.Empty;
        public string? GmailID { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public string? EmailID { get; set; } = string.Empty;
        public int PrimaryFlag { get; set; }
        public int ActiveFlag { get; set; }
        public int DistrictID { get; set; }
        public int SchoolID { get; set; }
        public string? ImageID { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? GroupName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Pwd { get; set; } = string.Empty;
        public string? Source { get; set; } = string.Empty;
        public string? Districtname { get; set; } = string.Empty;
        public string? HostName { get; set; } = string.Empty;
        public string? DomainName { get; set; } = string.Empty;
        public int ActivityBy { get; set; } = 0;
        public List<StudentObject>? objStudent { get; set; } = null;
    }
    public class StudentObject
    {
        public int LoginID { get; set; }
        public string? FullName { get; set; }
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? GmailID { get; set; }
        public string? Pin { get; set; }
    }
}
