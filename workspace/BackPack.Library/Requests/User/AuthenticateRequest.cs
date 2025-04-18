using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class AuthenticateRequest
    {
        [SwaggerSchema("StudentUser login name")]
        [SwaggerSchemaExample("DA1")]
        public string? LoginName { get; set; }
        [SwaggerSchema("Login password")]
        [SwaggerSchemaExample("1234")]
        public string? Password { get; set; }
        [SwaggerSchema("District name")]
        [SwaggerSchemaExample("pisd")]
        public string? DomainName { get; set; }
    }
}
