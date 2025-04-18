using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.District
{
    public class GetAllActiveDomainsResponse : ReadBaseResponse
    {
        public List<GetAllActiveDomainsResponseData>? Data { get; set; }
    }
    public class GetAllActiveDomainsResponseData
    {
        public List<GetAllPublicActiveDomainsResult>? GetAllActiveDomainsResult { get; set; }
    }
}
