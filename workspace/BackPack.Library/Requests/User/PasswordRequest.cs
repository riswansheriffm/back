
using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class PasswordRequest
    {
        [SwaggerSchema("District name")]
        [SwaggerSchemaExample("TestISD")]
        public string DistrictName { get; set; } = string.Empty;

        [SwaggerSchema("Type of user")]
        [SwaggerSchemaExample("Teacher")]
        public string UserType { get; set; } = string.Empty;

        [SwaggerSchema("User ID from")]
        [SwaggerSchemaExample("1")]
        public int FromUserID { get; set; }

        [SwaggerSchema("User ID to")]
        [SwaggerSchemaExample("50")]
        public int ToUserID { get; set; }
    }
}
