using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class ResetPasswordRequest
    {
        [SwaggerSchema("User ID")]
        [SwaggerSchemaExample("0")]
        public int LoginID { get; set; }

        [SwaggerSchema("User login name")]
        [SwaggerSchemaExample("WilliamMichael")]
        public string? LoginName { get; set; }

        [SwaggerSchema("User password")]
        [SwaggerSchemaExample("William#01Michael")]
        public string? Password { get; set; }

        [SwaggerSchema("Security code")]
        [SwaggerSchemaExample("WilliamMichael")]
        public string? Securitycode { get; set; }

        [SwaggerSchema("District name")]
        [SwaggerSchemaExample("PISD")]
        public string? DistrictName { get; set; }
    }
}
