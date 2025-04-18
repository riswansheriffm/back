using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class GetDomainAcceptedEvent : ReadBaseResponse
    {
        public DistrictResponseData? Data { get; set; }
    }

    public class DistrictResponseData
    {
        public GetDistrictResult? GetDistrictResult { get; set; }
    }
    public class GetDistrictResult
    {
        public DistrictObject? UO { get; set; }
        public string? Desc { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public int ID { get; set; } = 0;
        public string? StreetAddress { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? AccessType { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Zip { get; set; } = string.Empty;
        public int MaxStudents { get; set; } = 0;
        public int MaxTeachers { get; set; } = 0;
        public string? AccessToken { get; set; } = string.Empty;
        public string? ApplicationID { get; set; } = string.Empty;
        public string? Source { get; set; } = string.Empty;
        public string? SourceID { get; set; } = string.Empty;
    }

    public class DistrictObject
    {
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? PhoneNo { get; set; }
        public int LoginID { get; set; }
        public int DistrictID { get; set; }
    }
}
