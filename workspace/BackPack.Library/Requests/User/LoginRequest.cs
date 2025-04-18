using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class LoginRequest
    {
        [SwaggerSchema("User login name")]
        [SwaggerSchemaExample("TestTenantTeacher0001")]
        public string? LoginName { get; set; }

        [SwaggerSchema("Login password")]
        [SwaggerSchemaExample("TestT#123")]
        public string? Password { get; set; }

        [SwaggerSchema("District name")]
        [SwaggerSchemaExample("DName001")]
        public string DistrictName { get; set; } = string.Empty;
    }
}
