using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.School
{
    public class AllSchoolResponse : ReadBaseResponse
    {
        public List<AllResponseData>? Data { get; set; }
    }
    public class AllResponseData
    {
        public List<GetAllSchoolsResult>? GetAllSchoolsResult { get; set; }
    }
    public class GetAllSchoolsResult
    {
        public string? SchoolDesc { get; set; }
        public string? SchoolName { get; set; }
        public int SchoolID { get; set; }
    }
}
