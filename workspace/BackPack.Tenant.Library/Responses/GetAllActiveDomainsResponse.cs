using BackPack.Dependency.Library.Responses;

namespace BackPack.Tenant.Library.Responses
{
    public class GetAllActiveDomainsResponse : ReadBaseResponse
    {
        public GetAllActiveDomainsResponseData? Data { get; set; }
    }
    public class GetAllActiveDomainsResponseData
    {
        public List<GetAllPublicActiveDomainsResult>? GetAllActiveDomainsResult { get; set; }
    }
}
