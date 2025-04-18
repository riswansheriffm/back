using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Responses
{
    public class GetAllDistrictResponse : ReadBaseResponse
    {
        public AllDistrictsResponseData? Data { get; set; }
    }
    public class AllDistrictsResponseData
    {
        public List<GetAllDistrictsResult>? GetAllDistrictsResults { get; set; }
    }
    public class GetAllDistrictsResult
    {
        public Guid TenantID { get; set; }
        public string? Desc { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public int ID { get; set; }
        public string? AccessType { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Source { get; set; } = string.Empty;
        public string? SourceID { get; set; } = string.Empty;
    }
}
