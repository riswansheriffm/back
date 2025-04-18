
using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.ClassLink
{
    public class ClassLinkResponse : ReadBaseResponse
    {
        public ClassLinkResponseData? Data { get; set; }
    }
    public class ClassLinkResponseData
    {
        public string? SourceID { get; set; } = string.Empty;
        public string? ApplicationID { get; set; } = string.Empty;
        public string? AccessToken { get; set; } = string.Empty;
        public string? DomainName { get; set; } = string.Empty;
    }

}
