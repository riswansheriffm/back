using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.District
{
    public class AllPublicActiveDomainsResultResponse : ReadBaseResponse
    {
        public List<AllPublicActiveDomainsResultResponseData>? Data { get; set; }
    }
    public class AllPublicActiveDomainsResultResponseData
    {
        public List<GetAllPublicActiveDomainsResult>? GetAllPublicActiveDomainsResult { get; set; }
    }
    public class GetAllPublicActiveDomainsResult
    {
        public int DomainID { get; set; }
        public string? DomainName { get; set; }
    }
}
