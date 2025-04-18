using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Tenant.Library.Requests
{
    public class RefreshTokenRequest
    {
        [SwaggerSchema("User ID")]
        [SwaggerSchemaExample("44333")]
        public int UserID { get; set; }

        [SwaggerSchema("User Type")]
        [SwaggerSchemaExample("Student")]
        public string? UserType { get; set; }

        [SwaggerSchema("Refresh Token")]
        [SwaggerSchemaExample("8ocRyJ0pJx+0W7vOPzBh7Q46JYZmabcmNvwCjc4OWRo=")]
        public string? RefreshToken { get; set; }
    }
}
