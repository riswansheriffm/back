using BackPack.Dependency.Library.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace BackPack.Library.Requests.User
{
    public class UserRequest
    {
        [SwaggerSchema("DistrictID")]
        [SwaggerSchemaExample("104")]
        public int DistrictID { get; set; }
        [SwaggerSchema("SchoolID")]
        [SwaggerSchemaExample("160")]
        public int SchoolID { get; set; }
        [SwaggerSchema("ActivityBy")]
        [SwaggerSchemaExample("104")]
        public int ActivityBy { get; set; }
        [SwaggerSchema("Active Flag")]
        [SwaggerSchemaExample("0")]
        public int ActiveFlag { get; set; }
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
        [SwaggerSchemaExample("CurriculumAdmin")]
        public string? UserType { get; set; }
        [SwaggerSchema("Primary Flag")]
        [SwaggerSchemaExample("0")]
        public int PrimaryFlag { get; set; }
        [SwaggerSchema("Gmail address")]
        public string? GmailID { get; set; }
        [SwaggerSchema("Contact number")]
        public string? PhoneNo { get; set; }
        [SwaggerSchema("Domain Name")]
        public string? DomainName { get; set; }

    }
}
