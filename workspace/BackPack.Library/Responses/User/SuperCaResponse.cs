using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class SuperCaResponse : ReadBaseResponse
    {
        public SuperCaResult Data { get; set; } = new();
    }

    public class SuperCaResult
    {
        public SuperCaData GetSuperCaResult { get; set; } = new();
    }

    public class SuperCaData
    {
        public int ID { get; set; } = 0;
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
    }
}
