using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class UserUploadRequest
    {
        [SwaggerSchema("District ID")]
        [SwaggerSchemaExample("104")]
        public int DistrictID { get; set; }

        [SwaggerSchema("School ID")]
        [SwaggerSchemaExample("160")]
        public int SchoolID { get; set; }

        [SwaggerSchema("User ID")]
        [SwaggerSchemaExample("104")]
        public int ActivityBy { get; set; }

        [SwaggerSchema("URL")]
        [SwaggerSchemaExample("localhost")]
        public string? HostName { get; set; }

        [SwaggerSchema("User list")]
        public List<UserListUploadRequest> UserList { get; set; } = new List<UserListUploadRequest>();
    }
}
