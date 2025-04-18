using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Tenant.Library.Requests
{
    public class LoginRequest
    {
        [SwaggerSchema("User login name")]
        [SwaggerSchemaExample("LoginName")]
        public string? LoginName { get; set; }

        [SwaggerSchema("Login password")]
        [SwaggerSchemaExample("Password")]
        public string? Password { get; set; }

        [SwaggerSchema("District name")]
        [SwaggerSchemaExample("Domain")]
        public string DistrictName { get; set; } = string.Empty;
    }
}
