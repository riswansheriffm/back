using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.Token
{
    public class RefreshTokenRequest
    {
        [SwaggerSchema("Tenant ID")]
        [SwaggerSchemaExample("39CEEF34-8D3F-4E76-B259-E4C4F3D44DD9")]
        public Guid TenantID { get; set; }

        [SwaggerSchema("User ID")]
        [SwaggerSchemaExample("3327")]
        public int UserID { get; set; }

        [SwaggerSchema("User Type")]
        [SwaggerSchemaExample("Student")]
        public string? UserType { get; set; }

        [SwaggerSchema("Refresh Token")]
        [SwaggerSchemaExample("8ocRyJ0pJx+0W7vOPzBh7Q46JYZmabcmNvwCjc4OWRo=")]
        public string? RefreshToken { get; set; }
    }
}
