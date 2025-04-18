using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class UpdateUserRequest
    {
        [SwaggerSchema("LoginName")]
        [SwaggerSchemaExample("Login Name")]
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
        [SwaggerSchemaExample("CurriculumAdmin")]
        public string? UserType { get; set; }
        [SwaggerSchema("Primary Flag")]
        [SwaggerSchemaExample("0")]
        public int PrimaryFlag { get; set; }
        [SwaggerSchema("Gmail address")]
        public string? GmailID { get; set; }
        [SwaggerSchema("Contact number")]
        public string? PhoneNo { get; set; }
        [SwaggerSchema("DomainID")]
        [SwaggerSchemaExample("10")]
        public int? DomainID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("10")]
        public int? ActivityBy { get; set; }
    }
}
