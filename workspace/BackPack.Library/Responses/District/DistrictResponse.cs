using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.District
{
    public class ODistrictResponse : ReadBaseResponse
    {
        public ODistrictResponseData? Data { get; set; }
    }
    public class ODistrictResponseData
    {
        public OGetDistrictResult? GetDistrictResult { get; set; }
    }
    public class OGetDistrictResult
    {
        public List<ODistrictDetail>? DistrictDetails;
        public List<ODistrictObject>? UO { get; set; }
        public string? DomainDesc { get; set; } = string.Empty;
        public string? DomainName { get; set; } = string.Empty;
        public int DomainID { get; set; } = 0;
        public string? StreetAd { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? AccessType { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Zip { get; set; } = string.Empty;
        public int MaxStudents { get; set; } = 0;
        public int MaxTeachers { get; set; } = 0;
    }
    public class ODistrictDetail
    {
        public string? DomainDesc;
        public string? DomainName;
        public int DomainID;
        public string? StreetAd;
        public string? State;
        public string? AccessType;
        public string? City;
        public string? Zip;
        public int MaxStudents;
        public int MaxTeachers;
    }
    public class ODistrictObject
    {
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? PhoneNo { get; set; }
        public int ID { get; set; }
        public int DomainID { get; set; }
    }
}
