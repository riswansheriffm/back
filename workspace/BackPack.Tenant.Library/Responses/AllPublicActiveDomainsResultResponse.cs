using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Responses
{
    public class AllPublicActiveDomainsResultResponse : ReadBaseResponse
    {
        public AllPublicActiveDomainsResultResponseData? Data { get; set; }
    }
    public class AllPublicActiveDomainsResultResponseData
    {
        public List<GetAllPublicActiveDomainsResult>? GetAllPublicActiveDomainsResult { get; set; }
    }
    public class GetAllPublicActiveDomainsResult
    {
        public Guid TenantID { get; set; }
        public int DomainID { get; set; }
        public string? DomainName { get; set; }
    }
}
