using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.District
{
    public class GetAllDistrictResponse : ReadBaseResponse
    {
        public List<AllDistrictsResponseData>? Data { get; set; }
    }
    public class AllDistrictsResponseData
    {
        public List<GetAllDistrictsResult>? GetAllDistrictsResults { get; set; }
    }
    public class GetAllDistrictsResult
    {
        public string? DomainDesc { get; set; } = string.Empty;
        public string? DomainName { get; set; } = string.Empty;
        public int DomainID { get; set; }
        public string? AccessType { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
