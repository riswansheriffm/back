using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class DeleteUserRequest
    {
        [SwaggerSchema("District ID")]
        [SwaggerSchemaExample("7489")]
        public int? ID { get; set; }
        [SwaggerSchema("LoginName")]
        [SwaggerSchemaExample("Login Name")]
        public string? LoginName { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
    }
}
