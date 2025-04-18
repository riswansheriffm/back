using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class UserListUploadRequest
    {
        [SwaggerSchema("User login name")]
        [SwaggerSchemaExample("William")]
        public string? LoginName { get; set; }

        [SwaggerSchema("First name")]
        [SwaggerSchemaExample("William")]
        public string? FName { get; set; }

        [SwaggerSchema("Last name")]
        [SwaggerSchemaExample("Michael")]
        public string? LName { get; set; }

        [SwaggerSchema("Email address")]
        public string? EmailID { get; set; }

        [SwaggerSchema("User type")]
        [SwaggerSchemaExample("Student")]
        public string? UserType { get; set; }

        [SwaggerSchema("Class name")]
        [SwaggerSchemaExample("Science8")]
        public string? ClassName { get; set; }

        [SwaggerSchema("Gmail address")]
        public string? GmailID { get; set; }

        [SwaggerSchema("Contact number")]
        public string? PhoneNo { get; set; }
    }
}
