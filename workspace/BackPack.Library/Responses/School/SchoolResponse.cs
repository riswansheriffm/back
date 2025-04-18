using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.School
{
    public class SchoolResponse : ReadBaseResponse
    {
        public SchoolResponseData? Data { get; set; }
    }
    public class SchoolResponseData
    {
        public GetSchoolResult? GetSchoolResult { get; set; }
    }
    public class GetSchoolResult
    {
        public string? SchoolDesc { get; set; }
        public string? SchoolName { get; set; }
        public int SchoolID { get; set; }
        public List<SchoolsList>? UO { get; set; }
    }
    public class SchoolsList
    {
        public int ID { get; set; }
        public string? LoginName { get; set; }
        public string? FName { get; set; }
        public string? Lname { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? PhoneNo { get; set; }
        public int DomainID { get; set; }
    }
}
